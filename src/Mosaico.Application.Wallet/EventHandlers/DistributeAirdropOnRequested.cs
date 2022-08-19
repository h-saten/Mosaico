using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(DistributeAirdropOnRequested),  "projects:api")]
    [EventTypeFilter(typeof(DistributeAirdropEvent))]   
    public class DistributeAirdropOnRequested : EventHandlerBase
    {
        private readonly IProjectManagementClient _projectManagement;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly ITokenService _tokenService;
        private readonly IDateTimeProvider _provider;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IBusinessManagementClient _businessManagement;
        private readonly IUserManagementClient _managementClient;
        private readonly IUserWalletService _userWalletService;
        private readonly IOperationService _operationService;

        public DistributeAirdropOnRequested(IProjectManagementClient projectManagement, IWalletDbContext walletDbContext, IEthereumClientFactory ethereumClientFactory, ITokenService tokenService, IWalletDispatcher walletDispatcher, IDateTimeProvider provider, IBusinessManagementClient businessManagement, IUserManagementClient managementClient, IUserWalletService userWalletService, IOperationService operationService)
        {
            _projectManagement = projectManagement;
            _walletDbContext = walletDbContext;
            _ethereumClientFactory = ethereumClientFactory;
            _tokenService = tokenService;
            _walletDispatcher = walletDispatcher;
            _provider = provider;
            _businessManagement = businessManagement;
            _managementClient = managementClient;
            _userWalletService = userWalletService;
            _operationService = operationService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<DistributeAirdropEvent>();
            if(data != null)
            {
                var airdrop = await _projectManagement.GetProjectAirdropAsync(data.AirdropId);
                if (airdrop == null) return;
                if (!airdrop.TokenId.HasValue)
                {
                    return;
                }

                var validParticipants = airdrop.Participants.Where(p => p.Claimed && !p.WithdrawnAt.HasValue);
                var usersToReward = validParticipants
                    .Select(a => a.WalletAddress).ToList();
                var amountsToSend = validParticipants.Select(p => p.ClaimedTokenAmount).ToList();
                var userIds = airdrop.Participants.Where(p => p.Claimed && !p.WithdrawnAt.HasValue)
                    .Select(a => a.UserId.Trim().ToLower()).ToList();
                var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == airdrop.TokenId);
                CompanyWallet company;
                var transactionHash = string.Empty;

                var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o =>
                    o.Type == BlockchainOperationType.AIRDROP_DISTRIBUTION && o.Network == token.Network &&
                    (o.State == OperationState.PENDING || o.State == OperationState.IN_PROGRESS || o.State == OperationState.SUCCESSFUL) && o.CompanyId == token.CompanyId && o.TransactionId == airdrop.Id);
                
                if (operation != null)
                {
                    throw new AnotherAirdropInProgressException(airdrop.Id);
                }

                operation = await _operationService.CreateAirdropOperationAsync(airdrop.Id, token.Address, token.Network, token.CompanyId, data.UserId);
                    
                try
                {
                    if (token == null || string.IsNullOrWhiteSpace(token.Address))
                    {
                        throw new TokenNotFoundException(airdrop.TokenId.Value);
                    }

                    company = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                        c.CompanyId == token.CompanyId && c.Network == token.Network);
                    if (company == null)
                    {
                        throw new CompanyWalletNotFoundException(token.CompanyId.ToString());
                    }

                    var client = _ethereumClientFactory.GetClient(token.Network);
                    var account = await client.GetAccountAsync(company.PrivateKey);
                    transactionHash = await _tokenService.BatchTransferAsync(token.Network, account,
                        token.Address, usersToReward, amountsToSend);
                    await _operationService.SetTransactionInProgress(operation.Id, transactionHash);
                    var receipt = await client.GetTransactionAsync(transactionHash);
                    if (receipt.Status != 1)
                    {
                        throw new Exception($"Transaction failed on blockchain");
                    }

                    await _operationService.SetTransactionOperationCompleted(operation.Id);
                    await _projectManagement.SetAirdropWithdrawnAsync(airdrop.Id, userIds, transactionHash, CancellationToken.None);
                    await _walletDispatcher.AirdropDispatched(data.UserId, transactionHash);
                    try
                    {
                        foreach (var userWallet in usersToReward)
                        {
                            await _userWalletService.AddTokenToWalletAsync(userWallet, token.Address, token.Network);
                        }
                        await AddUserTransactionAsync(company, transactionHash, airdrop.TokensPerParticipant, userIds, token);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                catch (Exception ex)
                {
                    await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                    await _walletDispatcher.AirdropDispatchFailed(data.UserId, ex.Message);
                    return;
                }
            }
        }
        
        private async Task AddUserTransactionAsync(CompanyWallet companyWallet, string transactionHash, decimal tokenAmount, List<string> userIds, Token token)
        {
            if (companyWallet != null && !string.IsNullOrWhiteSpace(companyWallet?.AccountAddress))
            {
                var transactionStatus = await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(ts =>
                    ts.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed);
                var transactionType =
                    await _walletDbContext.TransactionType.FirstOrDefaultAsync(tp =>
                        tp.Key == Domain.Wallet.Constants.TransactionType.Transfer);
                var company = await _businessManagement.GetCompanyAsync(companyWallet.CompanyId);
                
                foreach (var userId in userIds)
                {
                    var userWallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w =>
                        w.UserId == userId && w.Network == token.Network);
                    if (userWallet != null)
                    {
                        if (!userWallet.Tokens.Any(w => w.TokenId == token.Id))
                        {
                            userWallet.Tokens.Add(new WalletToToken
                            {
                                Token = token,
                                TokenId = token.Id
                            });
                        }

                        var user = await _managementClient.GetUserAsync(userWallet.UserId);

                        var transaction = new Transaction
                        {
                            To = userWallet.AccountAddress,
                            Network = userWallet.Network,
                            From = companyWallet.AccountAddress,
                            Status = transactionStatus,
                            StatusId = transactionStatus.Id,
                            Type = transactionType,
                            TypeId = transactionType.Id,
                            FinishedAt = _provider.Now(),
                            TokenAmount = tokenAmount,
                            TokenId = token.Id,
                            CorrelationId = Guid.NewGuid().ToString(),
                            PaymentProcessor = Constants.PaymentProcessors.Mosaico,
                            WalletAddress = companyWallet.AccountAddress,
                            InitiatedAt = _provider.Now(),
                            FromDisplayName = company?.Name,
                            ToDisplayName = user?.Email,
                            TransactionHash = transactionHash
                        };
                        _walletDbContext.Transactions.Add(transaction);

                        await _walletDbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}