namespace Mosaico.Domain.Wallet.Entities.Enums
{
    public enum VestingFundStatus
    {
        Pending = 0,
        Deployed,
        Withdrawn,
        Deploying,
        Failed
    }
}