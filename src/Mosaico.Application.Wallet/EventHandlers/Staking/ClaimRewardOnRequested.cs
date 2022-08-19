using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers.Staking
{
    [EventInfo(nameof(ClaimRewardOnRequested),  "wallets:api")]
    [EventTypeFilter(typeof(ClaimReward))]
    public class ClaimRewardOnRequested : EventHandlerBase
    {
        private readonly IOperationService _operationService;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IWalletEmailService _walletEmailService;
        private readonly IUserManagementClient _managementClient;
        private readonly ILogger _logger;
        
        public ClaimRewardOnRequested(IOperationService operationService, IWalletDbContext walletDbContext, ILogger logger, IEthereumClientFactory ethereumClientFactory, IWalletDispatcher walletDispatcher, IWalletEmailService walletEmailService, IUserManagementClient managementClient)
        {
            _operationService = operationService;
            _walletDbContext = walletDbContext;
            _logger = logger;
            _ethereumClientFactory = ethereumClientFactory;
            _walletDispatcher = walletDispatcher;
            _walletEmailService = walletEmailService;
            _managementClient = managementClient;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<ClaimReward>();
            if(data == null) return;

            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == data.OperationId);
            if (operation == null || operation.State == OperationState.SUCCESSFUL) return;
            var client = _ethereumClientFactory.GetClient(operation.Network);
            var pair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(t => t.Id == data.PairId);
            if (pair == null) return;
            
            var claimHistory = new StakingClaimHistory
            {
                Amount = data.Amount,
                StakingPair = pair,
                StakingPairId = pair.Id,
                ClaimedAt = DateTimeOffset.UtcNow,
                UserId = data.UserId
            };
            await _walletDbContext.StakingClaimHistory.AddAsync(claimHistory);
            
            try
            {
                var receipt = await client.GetTransactionAsync(operation.TransactionHash);
                if (receipt.Status != 1)
                {
                    throw new Exception($"Transaction failed on blockchain");
                }

                await _walletDbContext.SaveChangesAsync();
                await _operationService.SetTransactionOperationCompleted(operation.Id);
                await _walletDispatcher.StakeRewardClaimed(data.UserId, receipt.TransactionHash);
                await SendEmailAsync(data.UserId, pair, data.Amount);
            }
            catch (Exception ex)
            {
                _walletDbContext.StakingClaimHistory.Remove(claimHistory);
                await _walletDbContext.SaveChangesAsync();
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                await _walletDispatcher.StakeRewardClaimFailed(data.UserId, ex.Message);
                throw;
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