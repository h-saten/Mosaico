namespace Mosaico.Blockchain.Base.DAL.Models
{
    public class TransactionReceipt
    {
        public string TransactionHash { get; set; }
        public string BlockHash { get; set; }
        public ulong BlockNumber { get; set; }
    }
}