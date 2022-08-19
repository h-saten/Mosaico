using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Blockchain.Ethereum.Exceptions
{
    public class InvalidNetworkException : ExceptionBase
    {
        public InvalidNetworkException(string message) : base($"Chain {message} cannot be identified")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidNetwork;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}