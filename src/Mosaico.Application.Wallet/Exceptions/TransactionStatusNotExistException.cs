using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class TransactionStatusNotExistException : ExceptionBase
    {
        public TransactionStatusNotExistException(string status) : base($"Transaction status: '{status}' not exist")
        {
        }

        public override string Code => Constants.ErrorCodes.TransactionStatusNotExist;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}