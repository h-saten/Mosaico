namespace Mosaico.Graph.Wallet.Entities
{
    public class WalletBalance
    {
        public Wallet Wallet { get; set; }
        public string ContractAddress { get; set; }
        public decimal Balance { get; set; }
        public string UserId { get; set; }
    }
}