using System;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IOperationService
    {
        Task<Operation> GetLatestOperationAsync(Guid transactionId);
        Task SetTransactionOperationCompleted(Guid operationId, decimal? gasUsed = null, decimal? payed = null, string hash = null);
        Task SetTransactionOperationFailed(Guid operationId, string reason);
        Task SetTransactionInProgress(Guid operationId, string hash = null, Guid? transactionId = null);
        Task<Operation> CreateVestingDeploymentTransaction(string network, Guid? transactionId = null,  string contract = null, string userId = null);
        Task<Operation> CreateTransactionOperationAsync(string network, Guid? transactionId = null,  string hash = null, string account = null, string contract = null, Guid? projectId = null, string userId = null);
        Task<Operation> CreateTokenDeploymentOperation(string network, Guid tokenId, string companyWalletAccountAddress, string tokenAddress, string currentUserUserId);
        Task<Operation> CreateVaultDeploymentOperationAsync(string network, Guid tokenId, string userContextUserId);
        Task<Operation> CreateVaultTransferOperationAsync(string network, Guid vaultId, string userId);
        Task<Operation> CreateDAODeploymentOperationAsync(string network, Guid companyId, string userId);
        Task<Operation> CreateVaultDepositDeploymentAsync(string network, Guid distributionId, string userId);
        Task<Operation> CreatePurchaseOperationAsync(string userId, string network, string accountAddress, Guid projectId);
        Task<Operation> CreateStakingOperationAsync(string network, string userId, Guid pairId);
        Task<Operation> CreateStakeWithdrawalOperationAsync(string network, string userId, Guid pairId);
        Task<Operation> CreateStakeClaimOperationAsync(string network, string userId, Guid pairId);
        Task<Operation> CreateStakeDistributionOperationAsync(string network, string userId, Guid pairId);
        Task<Operation> CreateTransferOperationAsync(string userId, string network, string contract, string tokenSymbol);
        Task<Operation> CreateDaoTransferOperationAsync(Guid companyId, string userId, string network, string contractAddress, string symbol);
        Task<Operation> CreateAirdropOperationAsync(Guid airdropId, string contractAddress, string tokenNetwork, Guid tokenCompanyId, string userId);
        Task<Operation> CreateTransactionConfirmationAsync(string network, Guid transactionId, Guid? projectId, string account = null);
    }
}