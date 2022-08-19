using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class TransactionAlreadyExistsException : ExceptionBase
    {
        public TransactionAlreadyExistsException(string id) : base($"Transaction with id {id} already exists")
        {
        }

        public override string Code => "TRANSACTION_ALREADY_EXISTS";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}