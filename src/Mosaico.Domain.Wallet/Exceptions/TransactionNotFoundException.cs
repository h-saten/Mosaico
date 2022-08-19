using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class TransactionNotFoundException : ExceptionBase
    {
        public TransactionNotFoundException(string message) : base($"Transaction {message} not found")
        {
        }

        public override string Code => "TRANSACTION_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}