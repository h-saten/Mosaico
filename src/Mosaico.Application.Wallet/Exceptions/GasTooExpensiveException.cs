using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class GasTooExpensiveException : ExceptionBase
    {
        public GasTooExpensiveException() : base($"Gas is too expensive")
        {
        }

        public override string Code => "GAS_TOO_EXPENSIVE";
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}