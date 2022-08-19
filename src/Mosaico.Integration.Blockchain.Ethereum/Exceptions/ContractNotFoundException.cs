using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Blockchain.Ethereum.Exceptions
{
    public class ContractNotFoundException : ExceptionBase
    {
        public ContractNotFoundException(string contractAddress) : base($"Contract at address: '{contractAddress}' not exist.", null, null)
        {
        }
        
        public ContractNotFoundException(string contractAddress, Exception innerException) : base($"Contract at address: '{contractAddress}' not exist.", null, innerException)
        {
        }

        public override string Code => Constants.ErrorCodes.ContractNotFound;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}