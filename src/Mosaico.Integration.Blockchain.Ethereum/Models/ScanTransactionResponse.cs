using System.Collections.Generic;

namespace Mosaico.Integration.Blockchain.Ethereum.Models
{
    public class ScanTransactionResponse
    {
        public ScanTransactionResponse()
        {
            Result = new List<ScanTransaction>();
        }
        
        public string Status { get; set; }
        public string Message { get; set; }
        public List<ScanTransaction> Result { get; set; }
    }
}