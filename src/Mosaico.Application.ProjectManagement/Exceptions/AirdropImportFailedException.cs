using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class AirdropImportFailedException : ExceptionBase
    {
        public AirdropImportFailedException(string message) : base($"Airdrop import failed because of {message}")
        {
        }
        public override string Code => "AIRDROP_IMPORT_FAILED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}