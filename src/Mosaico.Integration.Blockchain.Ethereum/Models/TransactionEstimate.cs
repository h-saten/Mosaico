namespace Mosaico.Integration.Blockchain.Ethereum.Models
{
    public class TransactionEstimate
    {
        public decimal GasPrice { get; set; }
        public decimal Gas { get; set; }
        public decimal TransactionFeeInETH { get; set; }
    }
}