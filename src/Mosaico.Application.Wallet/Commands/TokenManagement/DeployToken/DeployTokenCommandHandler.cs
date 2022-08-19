using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Queries.GetToken;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Integration.SignalR.DTO;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.DeployToken
{
    public class DeployTokenCommandHandler : IRequestHandler<DeployTokenCommand, string>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IDaoService _daoService;
        private readonly ICurrentUserContext _currentUser;
        private readonly IBackgroundJobProvider _backgroundJob;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IBusinessManagementClient _businessManagement;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IOperationService _operationService;
        private readonly ICacheClient _cacheClient;
        private readonly ILogger _logger;

        public DeployTokenCommandHandler(IWalletDbContext walletDbContext, IDaoService daoService, ICurrentUserContext currentUser, IBackgroundJobProvider backgroundJob, IWalletDispatcher walletDispatcher, IBusinessManagementClient businessManagement, ILogger logger, IEthereumClientFactory ethereumClientFactory, ICacheClient cacheClient, IOperationService operationService)
        {
            _walletDbContext = walletDbContext;
            _daoService = daoService;
            _currentUser = currentUser;
            _backgroundJob = backgroundJob;
            _walletDispatcher = walletDispatcher;
            _businessManagement = businessManagement;
            _logger = logger;
            _ethereumClientFactory = ethereumClientFactory;
            _cacheClient = cacheClient;
            _operationService = operationService;
        }

        public async Task<string> Handle(DeployTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId,
                cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            var operation = await _walletDbContext.Operations.OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync(o =>
                    o.Type == BlockchainOperationType.TOKEN_DEPLOYMENT && o.TransactionId == token.Id &&
                    o.Network == token.Network, cancellationToken: cancellationToken);
            
            if (operation != null && (operation.State == OperationState.SUCCESSFUL ||
                                      operation.State == OperationState.IN_PROGRESS))
            {
                throw new TokenDeployingException(token.Id);
            }
            
            if (token.Status == TokenStatus.Deployed)
            {
                throw new TokenAlreadyDeployedException(token.Id);
            }

            if (token.Status == TokenStatus.Deploying)
            {
                throw new TokenDeployingException(token.Id);
            }
            
            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(w => w.CompanyId == token.CompanyId && w.Network == token.Network, cancellationToken: cancellationToken);
            if (companyWallet == null)
            {
                throw new CompanyWalletNotFoundException(token.Id, token.Network);
            }

            var company = await _businessManagement.GetCompanyAsync(companyWallet.CompanyId, cancellationToken);
            if (company == null)
            {
                throw new CompanyWalletNotFoundException(companyWallet.CompanyId.ToString());
            }
            operation ??= await _operationService.CreateTokenDeploymentOperation(token.Network, token.Id, companyWallet.AccountAddress, token.Address, _currentUser.UserId);
            
            try
            {
                var transactionHash = await _daoService.CreateERC20Async(token.Network, new Daov1Configurations.CreateERC20Configuration
                {
                    Decimals = 18,
                    Name = token.Name,
                    Symbol = token.Symbol,
                    Url = "mosaico.ai",
                    DaoAddress = company.ContractAddress,
                    InitialSupply = token.TotalSupply,
                    IsBurnable = token.IsBurnable,
                    IsGovernance = token.IsGovernance,
                    IsMintable = token.IsMintable,
                    IsPaused = false,
                    PrivateKey = companyWallet.PrivateKey,
                    WalletAddress = companyWallet.AccountAddress
                });
                token.Status = TokenStatus.Deploying;
                token.ContractVersion = Integration.Blockchain.Ethereum.Constants.TokenContractVersions.Version1;
                token.OwnerAddress = companyWallet.AccountAddress;
                _walletDbContext.Tokens.Update(token);
                await _operationService.SetTransactionInProgress(operation.Id, transactionHash);
                await _walletDbContext.SaveChangesAsync(cancellationToken);
                _backgroundJob.Execute(() => DeployTokenAsync(token.Id, _currentUser.UserId, operation.Id));
                return transactionHash;
            }
            catch (Exception ex)
            {
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                token.Status = TokenStatus.Pending;
                _walletDbContext.Tokens.Update(token);
                _walletDbContext.Operations.Update(operation);
                await _walletDbContext.SaveChangesAsync(cancellationToken);
                await _walletDispatcher.DispatchTokenDeploymentFailed(_currentUser.UserId, ex.Message);
                throw;
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task DeployTokenAsync(Guid tokenId, string userId, Guid operationId)
        {
            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == operationId);
            if (operation == null)
            {
                throw new OperationNotFoundException(operationId.ToString());
            }
            
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == tokenId);
            if (token == null)
            {
                throw new TokenNotFoundException(tokenId);
            }

            try
            {
                var client = _ethereumClientFactory.GetClient(token.Network);
                var transactionReceipt = await client.GetTransactionAsync(operation.TransactionHash);
                if (transactionReceipt.Status == 1)
                {
                    var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(w => w.CompanyId == token.CompanyId && w.Network == token.Network);
                    if (companyWallet == null)
                    {
                        throw new CompanyWalletNotFoundException(token.Id, token.Network);
                    }

                    var company = await _businessManagement.GetCompanyAsync(companyWallet.CompanyId);
                    if (company == null)
                    {
                        throw new CompanyWalletNotFoundException(companyWallet.CompanyId.ToString());
                    }
                    
                    var tokens = await _daoService.GetTokensAsync(token.Network, company.ContractAddress, companyWallet.PrivateKey);
                    var tokenAddress = tokens.LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(tokenAddress))
                    {
                        token.Address = tokenAddress;
                        token.Status = TokenStatus.Deployed;
                        token.OwnerAddress = companyWallet.AccountAddress;
                        token.Status = TokenStatus.Deployed;
                        _walletDbContext.Tokens.Update(token);
                        await _walletDbContext.SaveChangesAsync();
                        await _operationService.SetTransactionOperationCompleted(operation.Id);
                        await _walletDispatcher.DispatchTokenDeployed(userId, new TokenCreatedDTO
                        {
                            TokenAddress = tokenAddress,
                            TokenId = token.Id
                        });
                        await _cacheClient.CleanAsync(new List<string>
                        {
                            $"{nameof(GetTokenQuery)}_{token.Id}"
                        });
                    }
                }
                else
                {
                    throw new Exception($"Transaction failed");
                }
            }
            catch (Exception ex)
            {
                _logger?.Error($"{ex.Message} / {ex.StackTrace}");
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                token.Status = TokenStatus.Pending;
                _walletDbContext.Tokens.Update(token);
                await _walletDbContext.SaveChangesAsync();
                await _walletDispatcher.DispatchTokenDeploymentFailed(userId, ex.Message);
                throw;
            }
        }
    }
}