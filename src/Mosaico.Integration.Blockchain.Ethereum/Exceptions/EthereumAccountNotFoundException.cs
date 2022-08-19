using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Blockchain.Ethereum.Exceptions
{
    public class EthereumAccountNotFoundException : ExceptionBase
    {
        public EthereumAccountNotFoundException(string message) : base(message)
        {
        }
        
        public EthereumAccountNotFoundException(string message, Exception innerException) : base(message, null, innerException)
        {
        }

        public override string Code => Constants.ErrorCodes.AccountNotFound;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}