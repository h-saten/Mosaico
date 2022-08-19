namespace Mosaico.Integration.Blockchain.Ethereum.Models
{
    public class ScanTransaction
    {
        public string TimeStamp { get; set; }
        public string Hash { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string BlockNumber { get; set; }
        public string Value { get; set; }
        public string ContractAddress { get; set; }
    }
}