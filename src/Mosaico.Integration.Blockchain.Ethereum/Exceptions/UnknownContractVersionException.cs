using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Blockchain.Ethereum.Exceptions
{
    public class UnknownContractVersionException : ExceptionBase
    {
        public UnknownContractVersionException(string contractType, string version) : base($"Cannot find version {version} for contract of type {contractType}")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidContractVersion;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}