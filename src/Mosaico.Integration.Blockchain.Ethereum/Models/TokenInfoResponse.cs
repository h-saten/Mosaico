using System.Collections.Generic;

namespace Mosaico.Integration.Blockchain.Ethereum.Models
{
    public class TokenInfoResponse
    {
        public TokenInfoResponse()
        {
            Result = new List<TokenDetails>();
        }
        
        public string Status { get; set; }
        public string Message { get; set; }
        public List<TokenDetails> Result { get; set; }
    }
}