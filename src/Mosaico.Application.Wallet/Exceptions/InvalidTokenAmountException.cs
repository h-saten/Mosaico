using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class InvalidTokenAmountException : ExceptionBase
    {
        public InvalidTokenAmountException() : base($"Invalid transaction token amount")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidTokenAmount;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}