using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Application.Wallet.Services
{
    public class OperationService : IOperationService
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IDateTimeProvider _timeProvider;

        public OperationService(IWalletDbContext walletDbContext, IDateTimeProvider timeProvider)
        {
            _walletDbContext = walletDbContext;
            _timeProvider = timeProvider;
        }

        public Task<Operation> GetLatestOperationAsync(Guid transactionId)
        {
            return _walletDbContext.Operations
                .Where(t => t.TransactionId == transactionId).OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<Operation> CreateTransactionConfirmationAsync(string network, Guid transactionId, Guid? projectId, string account = null)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.TRANSACTION_CONFIRMATION,
                AccountAddress = account,
                TransactionId = transactionId,
                ProjectId = projectId,
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateTransactionOperationAsync(string network, Guid? transactionId = null, string hash = null, string account = null, string contract = null, Guid? projectId = null, string userId = null)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.PURCHASE,
                UserId = userId,
                TransactionHash = hash,
                AccountAddress = account,
                ContractAddress = contract,
                TransactionId = transactionId,
                ProjectId = projectId,
            };
            if (!string.IsNullOrWhiteSpace(hash))
            {
                operation.State = OperationState.IN_PROGRESS;
            }
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateTokenDeploymentOperation(string network, Guid tokenId, string companyWalletAccountAddress,
            string tokenAddress, string currentUserUserId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.TOKEN_DEPLOYMENT,
                UserId = currentUserUserId,
                AccountAddress = companyWalletAccountAddress,
                ContractAddress = tokenAddress,
                TransactionId = tokenId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateVaultDeploymentOperationAsync(string network, Guid tokenId, string userContextUserId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.VAULT_DEPLOYMENT,
                UserId = userContextUserId,
                TransactionId = tokenId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateVaultTransferOperationAsync(string network, Guid vaultId, string userId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.VAULT_TRANSFER,
                UserId = userId,
                TransactionId = vaultId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateDAODeploymentOperationAsync(string network, Guid companyId, string userId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.DAO_DEPLOYMENT,
                UserId = userId,
                TransactionId = companyId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateVaultDepositDeploymentAsync(string network, Guid distributionId, string userId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.DEPOSIT_DEPLOYMENT,
                UserId = userId,
                TransactionId = distributionId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreatePurchaseOperationAsync(string userId, string network, string accountAddress, Guid projectId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.PURCHASE,
                UserId = userId,
                ProjectId = projectId,
                AccountAddress = accountAddress
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateStakingOperationAsync(string network, string userId, Guid pairId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.STAKING,
                UserId = userId,
                TransactionId = pairId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateStakeWithdrawalOperationAsync(string network, string userId, Guid pairId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.UNSTAKING,
                UserId = userId,
                TransactionId = pairId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateStakeClaimOperationAsync(string network, string userId, Guid pairId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.STAKE_CLAIMING,
                UserId = userId,
                TransactionId = pairId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateStakeDistributionOperationAsync(string network, string userId, Guid pairId)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.STAKE_DISTRIBUTE,
                UserId = userId,
                TransactionId = pairId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateTransferOperationAsync(string userId, string network, string contract, string tokenSymbol)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.TRANSFER,
                UserId = userId,
                ContractAddress = contract,
                ExtraData = tokenSymbol
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateDaoTransferOperationAsync(Guid companyId, string userId, string network, string contractAddress,
            string symbol)
        {
            var operation = new Operation
            {
                Network = network,
                CompanyId = companyId,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.DAO_TRANSFER,
                UserId = userId,
                ContractAddress = contractAddress,
                ExtraData = symbol
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateAirdropOperationAsync(Guid airdropId, string contractAddress, string tokenNetwork, Guid tokenCompanyId, string userId)
        {
            var operation = new Operation
            {
                TransactionId = airdropId,
                Network = tokenNetwork,
                CompanyId = tokenCompanyId,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.AIRDROP_DISTRIBUTION,
                ContractAddress = contractAddress,
                UserId = userId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task<Operation> CreateVestingDeploymentTransaction(string network, Guid? transactionId = null,  string contract = null, string userId = null)
        {
            var operation = new Operation
            {
                Network = network,
                State = OperationState.PENDING,
                Type = BlockchainOperationType.VESTING_CREATION,
                UserId = userId,
                ContractAddress = contract,
                TransactionId = transactionId
            };
            _walletDbContext.Operations.Add(operation);
            await _walletDbContext.SaveChangesAsync();
            return operation;
        }

        public async Task SetTransactionOperationCompleted(Guid operationId, decimal? gasUsed = null, decimal? payed = null, string hash = null)
        {
            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == operationId);
            if (operation == null) throw new OperationNotFoundException(operationId.ToString());
            if (!string.IsNullOrWhiteSpace(hash))
            {
                operation.TransactionHash = hash;
            }
            operation.GasUsed = gasUsed;
            operation.PayedNativeCurrency = payed;
            operation.State = OperationState.SUCCESSFUL;
            operation.FinishedAt = _timeProvider.Now();
            _walletDbContext.Operations.Update(operation);
            await _walletDbContext.SaveChangesAsync();
        }

        public async Task SetTransactionOperationFailed(Guid operationId, string reason)
        {
            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == operationId);
            if (operation == null) throw new OperationNotFoundException(operationId.ToString());
            operation.State = OperationState.FAILED;
            operation.FailureReason = reason;
            operation.FinishedAt = _timeProvider.Now();
            _walletDbContext.Operations.Update(operation);
            await _walletDbContext.SaveChangesAsync();
        }
        
        public async Task SetTransactionInProgress(Guid operationId, string hash = null, Guid? transactionId = null)
        {
            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == operationId);
            if (operation == null) throw new OperationNotFoundException(operationId.ToString());
            operation.State = OperationState.IN_PROGRESS;
            if (!string.IsNullOrWhiteSpace(hash))
            {
                operation.TransactionHash = hash;
            }

            if (transactionId.HasValue)
            {
                operation.TransactionId = transactionId;
            }

            _walletDbContext.Operations.Update(operation);
            await _walletDbContext.SaveChangesAsync();
        }
    }
}