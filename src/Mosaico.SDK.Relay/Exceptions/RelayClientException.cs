using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.SDK.Relay.Exceptions
{
    public class RelayClientException : ExceptionBase
    {
        public RelayClientException(string message) : base(message)
        {
        }

        public override string Code => "RELAY_EXCEPTION";
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}