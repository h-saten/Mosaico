using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class PurchaseFailedException : ExceptionBase
    {
        public PurchaseFailedException(string message) : base(message)
        {
        }

        public override string Code => "PURCHASE_FAILED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}