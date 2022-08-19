using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.EventHandlers.Staking
{
    [EventInfo(nameof(AwaitMetamaskClaimOnInitiated),  "wallets:api")]
    [EventTypeFilter(typeof(MetamaskClaimInitiated))]
    public class AwaitMetamaskClaimOnInitiated : EventHandlerBase
    {
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IWalletEmailService _walletEmailService;
        private readonly IUserManagementClient _managementClient;

        public AwaitMetamaskClaimOnInitiated(IEthereumClientFactory ethereumClientFactory, IWalletDbContext walletDbContext, IWalletDispatcher walletDispatcher, IWalletEmailService walletEmailService, IUserManagementClient managementClient)
        {
            _ethereumClientFactory = ethereumClientFactory;
            _walletDbContext = walletDbContext;
            _walletDispatcher = walletDispatcher;
            _walletEmailService = walletEmailService;
            _managementClient = managementClient;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event.GetData<MetamaskClaimInitiated>();
            if (data != null)
            {
                var pair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(t => t.Id == data.StakeId);
                if (pair != null)
                {
                    var stakes = await _walletDbContext.Stakings.Where(s =>
                        s.WalletType == StakingWallet.METAMASK && s.Wallet == data.Wallet &&
                        s.Status == StakingStatus.Deployed && s.UserId == data.UserId && s.StakingPairId == pair.Id)
                        .ToListAsync();
                    if (stakes.Any())
                    {
                        var stake = stakes.FirstOrDefault();
                        var client = _ethereumClientFactory.GetClient(stake.StakingPair.Network);
                        var result = await client.GetTransactionAsync(data.TransactionHash);
                        if (result != null && result.Status == 1)
                        {
                            var claimHistory = new StakingClaimHistory
                            {
                                TransactionHash = stake.TransactionHash,
                                StakingPair = pair,
                                StakingPairId = pair.Id,
                                ClaimedAt = DateTimeOffset.UtcNow,
                                UserId = data.UserId,
                                Amount = data.Amount
                            };
                            await _walletDbContext.StakingClaimHistory.AddAsync(claimHistory);
                            await _walletDispatcher.StakeRewardClaimed(data.UserId, result.TransactionHash);
                            await SendEmailAsync(data.UserId, pair, data.Amount);
                        }
                        else
                        {
                            await _walletDispatcher.StakeRewardClaimFailed(data.UserId, "Transaction failed");
                        }
                    }
                }
            }
        }
        
        private async Task SendEmailAsync(string userId, StakingPair stakingPair, decimal amount)
        {
            try
            {
                var user = await _managementClient.GetUserAsync(userId);
                if (user != null)
                {
                    var paymentCurrency = stakingPair.PaymentCurrencies.FirstOrDefault()?.PaymentCurrency;
                    if (paymentCurrency != null)
                    {
                        if (stakingPair.Type == StakingPairBaseCurrencyType.Token)
                        {
                            await _walletEmailService.SendStakingRewardPaid(stakingPair.StakingToken, user.Email, amount, paymentCurrency, user.Language);
                        }
                        else if (stakingPair.Type == StakingPairBaseCurrencyType.Currency)
                        {
                            await _walletEmailService.SendStakingRewardPaid(stakingPair.StakingPaymentCurrency, user.Email, amount, paymentCurrency, user.Language);
                        }
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