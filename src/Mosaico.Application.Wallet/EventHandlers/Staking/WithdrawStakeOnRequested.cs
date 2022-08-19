using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Abstractions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers.Staking
{
    [EventInfo(nameof(WithdrawStakeOnRequested),  "wallets:api")]
    [EventTypeFilter(typeof(WithdrawStake))]
    public class WithdrawStakeOnRequested : EventHandlerBase
    {
        private readonly IOperationService _operationService;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IWalletStakingService _walletStakingService;
        private readonly ITokenLockService _lockService;
        private readonly IUserManagementClient _managementClient;
        private readonly IWalletEmailService _walletEmailService;
        private readonly ILogger _logger;
        
        public WithdrawStakeOnRequested(IOperationService operationService, IWalletDbContext walletDbContext, ILogger logger, IEthereumClientFactory ethereumClientFactory, IWalletDispatcher walletDispatcher, IWalletStakingService walletStakingService, ITokenLockService lockService, IUserManagementClient managementClient, IWalletEmailService walletEmailService)
        {
            _operationService = operationService;
            _walletDbContext = walletDbContext;
            _logger = logger;
            _ethereumClientFactory = ethereumClientFactory;
            _walletDispatcher = walletDispatcher;
            _walletStakingService = walletStakingService;
            _lockService = lockService;
            _managementClient = managementClient;
            _walletEmailService = walletEmailService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<WithdrawStake>();
            if(data == null) return;

            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == data.OperationId);
            if (operation == null || operation.State == OperationState.SUCCESSFUL) return;
            var pair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(p => p.Id == data.PairId);
            if (pair == null) return;
            
            var stakes = await _walletDbContext.Stakings.Where(s => s.WalletType == StakingWallet.MOSAICO_WALLET &&
                s.Status == StakingStatus.Deployed && s.UserId == data.UserId && s.StakingPairId == data.PairId).ToListAsync();
            
            if (!stakes.Any())
            {
                throw new StakingNotFoundException(pair.Id);
            }

            var currentStake = stakes.FirstOrDefault();
            var client = _ethereumClientFactory.GetClient(operation.Network);
            
            try
            {
                var withdrawalBalance = await _walletStakingService.GetStakingBalanceAsync(currentStake);
                if (withdrawalBalance <= 0)
                {
                    throw new Exception($"Nothing to withdraw");
                }
                
                if (!pair.SkipApproval)
                {
                    var approvalTransactionHash = await _walletStakingService.ApproveWithdrawableTokenAsync(currentStake, withdrawalBalance);
                    await _operationService.SetTransactionInProgress(operation.Id, hash: approvalTransactionHash);
                    var approvalReceipt = await client.GetTransactionAsync(operation.TransactionHash);
                    if (approvalReceipt.Status != 1)
                    {
                        throw new Exception($"Transaction failed on blockchain");
                    }
                }
                var hash = await _walletStakingService.WithdrawAsync(stakes.FirstOrDefault());
                await _operationService.SetTransactionInProgress(operation.Id, hash: hash);
                var receipt = await client.GetTransactionAsync(operation.TransactionHash);
                if (receipt.Status != 1)
                {
                    throw new Exception($"Transaction failed on blockchain");
                }

                foreach (var stake in stakes)
                {
                    stake.Status = StakingStatus.Withdrawn;
                    var tokenLock = await _walletDbContext.TokenLocks.FirstOrDefaultAsync(t =>
                        t.LockReason == Constants.LockReasons.STAKE &&
                        !t.Expired && t.UserId == stake.UserId && t.Amount == stake.Balance);
                    if (tokenLock != null)
                    {
                        await _lockService.SetExpiredAsync(tokenLock);
                    }
                }

                await _walletDbContext.SaveChangesAsync();
                await _operationService.SetTransactionOperationCompleted(operation.Id);
                await _walletDispatcher.StakeWithdrawalSucceeded(data.UserId, receipt.TransactionHash);
                await SendEmailAsync(stakes);
            }
            catch (Exception ex)
            {
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                await _walletDispatcher.StakeWithdrawalFailed(data.UserId, ex.Message);
                throw;
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