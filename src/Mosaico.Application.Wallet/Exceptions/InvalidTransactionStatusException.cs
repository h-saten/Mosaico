using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class InvalidTransactionStatusException : ExceptionBase
    {
        public InvalidTransactionStatusException(string id) : base($"Transaction has invalid status")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidTransactionStatus;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}