using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Blockchain.Base.Exceptions
{
    public class UnsupportedContractVersionException : ExceptionBase
    {
        public UnsupportedContractVersionException(string contractType, string version) : base($"Contract {contractType} has no supported version {version}")
        {
        }

        public override string Code => Constants.ErrorCodes.UnsupportedContractVersion;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}