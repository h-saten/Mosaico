using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.BurnToken
{
    public class BurnTokenCommandHandler : IRequestHandler<BurnTokenCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly IDaoService _daoService;
        private readonly IBusinessManagementClient _businessManagement;
        private readonly ILogger _logger;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IBackgroundJobProvider _backgroundJob;

        public BurnTokenCommandHandler(IWalletDbContext walletDbContext, ICurrentUserContext currentUser, IDaoService daoService, IBusinessManagementClient businessManagement, IWalletDispatcher walletDispatcher, IBackgroundJobProvider backgroundJob, ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _currentUser = currentUser;
            _daoService = daoService;
            _businessManagement = businessManagement;
            _walletDispatcher = walletDispatcher;
            _backgroundJob = backgroundJob;
            _logger = logger;
        }

        public async Task<Unit> Handle(BurnTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            if (!token.IsBurnable)
            {
                throw new TokenNotMintableException(request.TokenId);
            }
            
            _backgroundJob.Execute(() => BurnTokenAsync(token.Id, _currentUser.UserId, request.Amount));
            
            return Unit.Value;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task BurnTokenAsync(Guid tokenId, string userId, decimal amount)
        {
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == tokenId);
            if (token == null)
            {
                throw new TokenNotFoundException(tokenId);
            }

            if (!token.IsBurnable)
            {
                throw new TokenNotMintableException(tokenId);
            }
            
            if (token.Status == TokenStatus.Deployed)
            {
                var companyWallet =
                    await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                        c.CompanyId == token.CompanyId && c.Network == token.Network);
                if (companyWallet == null)
                {
                    throw new CompanyWalletNotFoundException(token.CompanyId, token.Network);
                }
                
                var company = await _businessManagement.GetCompanyAsync(companyWallet.CompanyId);
                if (company == null)
                {
                    throw new CompanyWalletNotFoundException(companyWallet.CompanyId.ToString());
                }

                try
                {
                    await _daoService.BurnTokenAsync(token.Network, new Daov1Configurations.BurnTokenConfiguration
                    {
                        Amount = amount,
                        Decimals = 18,
                        ContractAddress = token.Address,
                        DaoAddress = companyWallet.AccountAddress,
                        PrivateKey = companyWallet.PrivateKey
                    });
                }
                catch (Exception ex)
                {
                    await _walletDispatcher.TokenBurningFailed(userId, ex.Message);
                    return;
                }
            }
            token.TotalSupply -= amount;
            await _walletDbContext.SaveChangesAsync();
            await _walletDispatcher.TokenBurned(userId, token.Id);
        }
    }
}