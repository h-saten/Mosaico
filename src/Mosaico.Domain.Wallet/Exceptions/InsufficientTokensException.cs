using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class InsufficientTokensException : ExceptionBase
    {
        public InsufficientTokensException() : base($"Insufficient tokens")
        {
        }

        public override string Code => "INSUFFICIENT_TOKENS";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}