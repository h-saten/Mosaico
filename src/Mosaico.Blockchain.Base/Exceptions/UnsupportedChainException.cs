using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Blockchain.Base.Exceptions
{
    public class UnsupportedChainException : ExceptionBase
    {
        public string Chain { get; set; }
        
        public UnsupportedChainException() : base("Chain is not supported.")
        {
        }        
        
        public UnsupportedChainException(string chain) : base($"Chain: '{chain}' is not supported.")
        {
            Chain = chain;
        }

        public override string Code => Constants.ErrorCodes.UnsupportedChain;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}