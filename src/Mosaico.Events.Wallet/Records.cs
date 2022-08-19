using System;
using System.Collections.Generic;

namespace Mosaico.Events.Wallet
{
    public record TokenLogoUploaded(Guid TokenId, string LogoUrl);
    public record TokenCreated(Guid TokenId, Guid? ProjectId);
    public record TokenImported(Guid TokenId, Guid? ProjectId);

    public record DAOCreatedEvent(Guid CompanyId, string Network);
    public record NonCustodialTransactionInitiated(Guid TransactionId, Guid OperationId);
    public record SuccessfulPurchaseEvent(string UserId, decimal TokenAmount, Guid StageId, Guid TransactionId, string PaymentProcessor);
    public record PurchaseFailedEvent(string UserId, decimal TokenAmount, Guid StageId, Guid TransactionId);
    public record BankTransferTransactionInitiatedEvent(string UserId, Guid TransactionId);
    public record BankTransferConfirmedEvent(string UserId, Guid TransactionId);
    public record NFTCollectionCreated(string UserId, Guid NFTCollectionId, string Address);
    public record NFTCreated(Guid NFTCollectionId, Guid NFTId, string TokenId);

    public record DeployVaultRequested(Guid TokenId, string UserId, Guid OperationId);
    public record MosaicoWalletPurchaseInitiated(Guid TransactionId, string UserId, Guid OperationId);

    public record VestingDeploymentRequested(Guid VestingId, string UserId);
    public record VaultSendRequested(Guid TokenDistributionId, decimal Amount, string Recipient, string UserId);
    public record CreateVaultDepositRequested(Guid TokenDistributionId, string UserId);

    public record StakeInitiated(Guid StakingId, Guid OperationId);
    public record TransactionsExpired(List<Guid> TransactionIds);

    public record WithdrawalConfirmed(string UserId, Guid PairId);
    public record WithdrawalFailed(string UserId, Guid PairId);

    public record StakeConfirmed(string UserId, Guid StakeId);

    public record StakeFailed(string UserId, Guid StakeId);
    public record ClaimConfirmed(string UserId, Guid PairId, decimal balance);

    public record ClaimFailed(string UserId, Guid PairId, decimal balance);
    public record WithdrawStake(Guid PairId, Guid OperationId, string UserId);

    public record ClaimReward(Guid PairId, Guid OperationId, string UserId, decimal Amount);

    public record Distribute(Guid PairId, Guid OperationId, string UserId, decimal Amount, Guid CompanyId);

    public record MetamaskStakeInitiated(Guid StakeId, string UserId);

    public record MetamaskWithdrawalInitiated(Guid StakeId, string UserId, string TransactionHash, string Wallet);

    public record StakingTermsUploaded(Guid StakingPairId, string DocumentUrl, string Language);

    public record MetamaskClaimInitiated(Guid StakeId, string UserId, string TransactionHash, string Wallet, decimal Amount);
}