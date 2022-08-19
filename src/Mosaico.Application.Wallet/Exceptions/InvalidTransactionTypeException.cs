using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class InvalidTransactionTypeException : ExceptionBase
    {
        public InvalidTransactionTypeException(string transactionType) : base($"Transaction type {transactionType} is not valid for this operation")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidTransactionType;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}