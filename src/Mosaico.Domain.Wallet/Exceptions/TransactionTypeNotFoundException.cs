using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class TransactionTypeNotFoundException : ExceptionBase
    {
        public TransactionTypeNotFoundException(string type) : base($"Transaction type {type} not found")
        {
        }

        public override string Code => "TRANSACTION_TYPE_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}