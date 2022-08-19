using System;
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
    [EventInfo(nameof(AwaitMetamaskStakeOnInitiated),  "wallets:api")]
    [EventTypeFilter(typeof(MetamaskStakeInitiated))]
    public class AwaitMetamaskStakeOnInitiated : EventHandlerBase
    {
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IWalletEmailService _walletEmailService;
        private readonly IUserManagementClient _managementClient;

        public AwaitMetamaskStakeOnInitiated(IEthereumClientFactory ethereumClientFactory, IWalletDbContext walletDbContext, IWalletDispatcher walletDispatcher, IWalletEmailService walletEmailService, IUserManagementClient managementClient)
        {
            _ethereumClientFactory = ethereumClientFactory;
            _walletDbContext = walletDbContext;
            _walletDispatcher = walletDispatcher;
            _walletEmailService = walletEmailService;
            _managementClient = managementClient;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event.GetData<MetamaskStakeInitiated>();
            if (data != null)
            {
                var stake = await _walletDbContext.Stakings.FirstOrDefaultAsync(t => t.Id == data.StakeId);
                if (stake != null && stake.Status != StakingStatus.Deployed && !string.IsNullOrWhiteSpace(stake.TransactionHash))
                {
                    var client = _ethereumClientFactory.GetClient(stake.StakingPair.Network);
                    var result = await client.GetTransactionAsync(stake.TransactionHash);
                    if (result != null && result.Status == 1)
                    {
                        stake.Status = StakingStatus.Deployed;
                        _walletDbContext.Stakings.Update(stake);
                        await _walletDbContext.SaveChangesAsync();
                        await _walletDispatcher.Staked(data.UserId, result.TransactionHash);
                        await SendEmailAsync(stake);
                    }
                    else
                    {
                        stake.Status = StakingStatus.Failed;
                        _walletDbContext.Stakings.Update(stake);
                        await _walletDbContext.SaveChangesAsync();
                        await _walletDispatcher.StakeFailed(data.UserId, "Transaction failed");
                    }
                }
            }
        }

        private async Task SendEmailAsync(Domain.Wallet.Entities.Staking.Staking stake)
        {
            try
            {
                var user = await _managementClient.GetUserAsync(stake.UserId);
                if (user != null)
                {
                    var tokenSymbol = stake.StakingPair.Type == StakingPairBaseCurrencyType.Token
                        ? stake.StakingPair.Token.Symbol
                        : stake.StakingPair.StakingPaymentCurrency.Ticker;
                    await _walletEmailService.SendStakingActivatedAsync(tokenSymbol, user.Email, stake.Balance,
                        stake.StakingPair.EstimatedAPR, user.Language);
                }
            }
            catch (Exception e)
            {
                //ignore
            }
        }
    }
}