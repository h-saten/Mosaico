using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class KPINotFoundException : ExceptionBase
    {
        public KPINotFoundException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.KPINotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}