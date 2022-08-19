using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Abstractions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers.Staking
{
    [EventInfo(nameof(DistributeStakeOnRequested),  "wallets:api")]
    [EventTypeFilter(typeof(Distribute))]
    public class DistributeStakeOnRequested : EventHandlerBase
    {
        private readonly IOperationService _operationService;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IWalletStakingService _walletStakingService;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        
        public DistributeStakeOnRequested(IOperationService operationService, IWalletDbContext walletDbContext, ILogger logger, IEthereumClientFactory ethereumClientFactory, IWalletDispatcher walletDispatcher, IWalletStakingService walletStakingService, ITokenService tokenService)
        {
            _operationService = operationService;
            _walletDbContext = walletDbContext;
            _logger = logger;
            _ethereumClientFactory = ethereumClientFactory;
            _walletDispatcher = walletDispatcher;
            _walletStakingService = walletStakingService;
            _tokenService = tokenService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<Distribute>();
            if(data == null) return;
            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == data.OperationId);
            if (operation == null || operation.State == OperationState.IN_PROGRESS) return;
            var pair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(p => p.Id == data.PairId);
            if(pair == null) return;
            var client = _ethereumClientFactory.GetClient(operation.Network);
            
            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                c.CompanyId == data.CompanyId && c.Network == pair.Network);
            if (companyWallet == null) return;
            var account = await client.GetAccountAsync(companyWallet.PrivateKey);
            
            try
            {
                var contract = pair.PaymentCurrencies?.FirstOrDefault()?.PaymentCurrency?.ContractAddress;
                if (string.IsNullOrWhiteSpace(contract)) throw new Exception($"Pair payment currency is missing");
                
                var approvalTransaction = await _tokenService.ApproveAsync(pair.Network, account, contract,
                    pair.ContractAddress, data.Amount);
                await _operationService.SetTransactionInProgress(operation.Id, approvalTransaction);
                var receipt = await client.GetTransactionAsync(approvalTransaction);
                if (receipt.Status != 1)
                {
                    //TODO: to custom exception
                    throw new Exception($"Approval transaction failed");
                }
                var hash = await _walletStakingService.DistributeAsync(pair, data.Amount, companyWallet.PrivateKey);
                await _operationService.SetTransactionInProgress(operation.Id, hash: hash);
                receipt = await client.GetTransactionAsync(operation.TransactionHash);
                if (receipt.Status != 1)
                {
                    throw new Exception($"Transaction failed on blockchain");
                }

                await _operationService.SetTransactionOperationCompleted(operation.Id);
                await _walletDispatcher.StakeDistributed(data.UserId, receipt.TransactionHash);
            }
            catch (Exception ex)
            {
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                await _walletDispatcher.StakeDistributionFailed(data.UserId, ex.Message);
                throw;
            }
        }
    }
}