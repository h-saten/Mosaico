using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Airdrop
{
    public class AirdropExhaustedException : ExceptionBase
    {
        public AirdropExhaustedException(string message) : base($"Airdrop {message} already exhausted")
        {
        }

        public override string Code => "AIRDROP_EXHAUSTED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}