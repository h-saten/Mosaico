using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Abstractions;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
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
    [EventInfo(nameof(StakeOnRequested),  "wallets:api")]
    [EventTypeFilter(typeof(StakeInitiated))]
    public class StakeOnRequested : EventHandlerBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IOperationService _operationService;
        private readonly IWalletStakingService _walletStakingService;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly ITokenLockService _tokenLockService;
        private readonly ILogger _logger;
        private readonly IWalletEmailService _emailService;
        private readonly IUserManagementClient _managementClient;

        public StakeOnRequested(IWalletDbContext walletDbContext, IWalletDispatcher walletDispatcher, ILogger logger, IOperationService operationService, IWalletStakingService walletStakingService, IEthereumClientFactory ethereumClientFactory, ITokenLockService tokenLockService, IWalletEmailService emailService, IUserManagementClient managementClient)
        {
            _walletDbContext = walletDbContext;
            _walletDispatcher = walletDispatcher;
            _logger = logger;
            _operationService = operationService;
            _walletStakingService = walletStakingService;
            _ethereumClientFactory = ethereumClientFactory;
            _tokenLockService = tokenLockService;
            _emailService = emailService;
            _managementClient = managementClient;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            _logger?.Information($"Received stake event");
            var data = @event.GetData<StakeInitiated>();
            if (data == null)
                return;
                
            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == data.OperationId);
            if(operation == null || operation.State == OperationState.IN_PROGRESS) 
                return;
            
            var stake = await _walletDbContext.Stakings.FirstOrDefaultAsync(t => t.Id == data.StakingId);
            if (stake == null)
            {
                _logger?.Warning($"Staking {data.StakingId} was not found");
                return;
            }

            var tokenLock = stake.StakingPair.Type == StakingPairBaseCurrencyType.Currency
                ? await _walletDbContext.TokenLocks.FirstOrDefaultAsync(t =>
                    t.PaymentCurrencyId == stake.StakingPair.StakingPaymentCurrencyId.Value &&
                    t.UserId == stake.UserId &&
                    t.LockReason == Constants.LockReasons.STAKE && t.Amount == stake.Balance) :
                await _walletDbContext.TokenLocks.FirstOrDefaultAsync(t =>
                    t.TokenId == stake.StakingPair.StakingTokenId.Value &&
                    t.UserId == stake.UserId &&
                    t.LockReason == Constants.LockReasons.STAKE && t.Amount == stake.Balance);

            try
            {
                if (!(await _walletStakingService.CanStakeAsync(stake)))
                {
                    _logger?.Warning($"User {stake.UserId} cannot stake right now");
                    throw new AnotherStakingInProgressException(stake.UserId);
                }
                
                var client = _ethereumClientFactory.GetClient(stake.StakingPair.Network);
                await _walletStakingService.StartDeploymentAsync(stake);
                var requireApproval = await _walletStakingService.RequiresApprovalAsync(stake);
                if (requireApproval)
                {
                    var approvalTransaction = await _walletStakingService.ApproveAsync(stake);
                    await _operationService.SetTransactionInProgress(operation.Id, approvalTransaction);
                    var approvalReceipt = await client.GetTransactionAsync(approvalTransaction);
                    if (approvalReceipt.Status != 1)
                    {
                        //TODO: to custom exception
                        throw new Exception($"Approval transaction failed");
                    }
                }
                else
                {
                    await _operationService.SetTransactionInProgress(operation.Id);
                }

                var stakeTransaction = await _walletStakingService.StakeAsync(stake);
                await _operationService.SetTransactionInProgress(operation.Id, stakeTransaction);
                var receipt = await client.GetTransactionAsync(stakeTransaction);
                if (receipt.Status != 1)
                {
                    //TODO: to custom exception
                    throw new Exception($"Staking transaction failed");
                }
                await _operationService.SetTransactionOperationCompleted(operation.Id, hash: stakeTransaction);
                await _walletStakingService.SetDeploymentCompleted(stake);
                await _walletDispatcher.Staked(stake.UserId, stakeTransaction);
                if (tokenLock != null)
                {
                    await _tokenLockService.SetExpiredAsync(tokenLock);
                }

                await SendEmailAsync(stake);
            }
            catch (Exception e)
            {
                if (tokenLock != null)
                {
                    await _tokenLockService.SetExpiredAsync(tokenLock);
                }
                await _operationService.SetTransactionOperationFailed(operation.Id, e.Message);
                await _walletStakingService.SetDeploymentFailed(stake, e.Message);
                _logger?.Error(e, "Error during staking");
                await _walletDispatcher.StakeFailed(stake.UserId, e.Message);
                throw;
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
                    await _emailService.SendStakingActivatedAsync(tokenSymbol, user.Email, stake.Balance,
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