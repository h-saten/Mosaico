using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.EventHandlers.Staking
{
    [EventInfo(nameof(AwaitMetamaskWithdrawOnInitiated),  "wallets:api")]
    [EventTypeFilter(typeof(MetamaskWithdrawalInitiated))]
    public class AwaitMetamaskWithdrawOnInitiated : EventHandlerBase
    {
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IUserManagementClient _managementClient;
        private readonly IWalletEmailService _walletEmailService;
        private readonly IWalletDispatcher _walletDispatcher;

        public AwaitMetamaskWithdrawOnInitiated(IEthereumClientFactory ethereumClientFactory, IWalletDbContext walletDbContext, IWalletDispatcher walletDispatcher, IUserManagementClient managementClient, IWalletEmailService walletEmailService)
        {
            _ethereumClientFactory = ethereumClientFactory;
            _walletDbContext = walletDbContext;
            _walletDispatcher = walletDispatcher;
            _managementClient = managementClient;
            _walletEmailService = walletEmailService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event.GetData<MetamaskWithdrawalInitiated>();
            if (data != null)
            {
                var normalizedWallet = data.Wallet.Trim().ToLowerInvariant();
                var stakes = await _walletDbContext.Stakings.Where(s =>
                    s.Status == StakingStatus.Deployed && s.UserId == data.UserId && s.StakingPairId == data.StakeId
                    && s.WalletType == StakingWallet.METAMASK && s.Wallet == normalizedWallet).ToListAsync();
                if (stakes.Any())
                {
                    var stakingPair = stakes.FirstOrDefault()?.StakingPair;
                    var client = _ethereumClientFactory.GetClient(stakingPair.Network);
                    var result = await client.GetTransactionAsync(data.TransactionHash);
                    if (result != null && result.Status == 1)
                    {
                        foreach (var stake in stakes)
                        {
                            stake.Status = StakingStatus.Withdrawn;
                            _walletDbContext.Stakings.Update(stake);
                        }

                        await _walletDbContext.SaveChangesAsync();
                        await _walletDispatcher.StakeWithdrawalSucceeded(data.UserId, result.TransactionHash);
                        await SendEmailAsync(stakes);
                    }
                    else
                    {
                        await _walletDispatcher.StakeWithdrawalFailed(data.UserId, "Transaction failed");
                    }
                }
            }
        }
        
        private async Task SendEmailAsync(List<Domain.Wallet.Entities.Staking.Staking> stakes)
        {
            try
            {
                var stake = stakes.FirstOrDefault();
                if (stake != null)
                {
                    var user = await _managementClient.GetUserAsync(stake.UserId);
                    if (user != null)
                    {
                        var tokenSymbol = stake.StakingPair.Type == StakingPairBaseCurrencyType.Token
                            ? stake.StakingPair.Token.Symbol
                            : stake.StakingPair.StakingPaymentCurrency.Ticker;
                        await _walletEmailService.SendStakingDeactivatedAsync(tokenSymbol, user.Email, stakes.Sum(s => s.Balance),
                            user.Language);
                    }
                }
            }
            catch (Exception e)
            {
                //ignore
            }
        }
    }
}