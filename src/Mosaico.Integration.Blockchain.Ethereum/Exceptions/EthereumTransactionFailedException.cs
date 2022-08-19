using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Blockchain.Ethereum.Exceptions
{
    public class EthereumTransactionFailedException : ExceptionBase
    {
        public EthereumTransactionFailedException() : base("Transaction has failed")
        {
        }
        
        public EthereumTransactionFailedException(string message) : base($"Transaction {message} has failed")
        {
        }

        public override string Code => Constants.ErrorCodes.TransactionFailed;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}