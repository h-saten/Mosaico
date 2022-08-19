using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Blockchain.Ethereum.Exceptions
{
    public class EtherScanException : ExceptionBase
    {
        public EtherScanException(string message) : base(message)
        {
        }

        public override string Code => "ETHERSCAN_EXCEPTION";
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}