namespace Mosaico.Domain.Wallet.Entities.Enums
{
    public enum BlockchainOperationType
    {
        UNKNOWN = 0,
        PURCHASE = 1,
        TRANSACTION_CONFIRMATION = 2,
        TRANSFER = 3,
        STAKING = 4,
        UNSTAKING = 5,
        VESTING = 6,
        TOKEN_DEPLOYMENT = 7,
        VAULT_DEPLOYMENT = 8,
        DEPOSIT_DEPLOYMENT = 9,
        VAULT_TRANSFER = 10,
        MINT = 11,
        BURN = 12,
        DAO_DEPLOYMENT = 13,
        VESTING_CREATION = 14,
        STAKE_CLAIMING = 15,
        STAKE_DISTRIBUTE = 16,
        DAO_TRANSFER = 17,
        AIRDROP_DISTRIBUTION = 18,
        TRANSACTION_REFUND = 19
    }
}