using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Airdrop
{
    public class NoTokenForAirdropException : ExceptionBase
    {
        public NoTokenForAirdropException(string message) : base($"Project {message} has no token assigned. Cannot create airdrop")
        {
        }

        public override string Code => "PROJECT_TOKEN_MISSING";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}