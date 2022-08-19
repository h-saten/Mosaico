using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class UnauthorizedPurchaseException : ExceptionBase
    {
        public UnauthorizedPurchaseException(string userId, string message = null) : base(message ?? $"Unauthorized purchase attempt by user {userId}")
        {
        }

        public override string Code => Constants.ErrorCodes.UnauthorizedPurchase;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}