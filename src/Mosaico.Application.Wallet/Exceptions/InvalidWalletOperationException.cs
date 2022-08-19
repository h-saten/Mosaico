using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class InvalidWalletOperationException : ExceptionBase
    {
        public InvalidWalletOperationException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidWalletOperation;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}