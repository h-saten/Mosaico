using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Airdrop
{
    public class AirdropAlreadyClaimException : ExceptionBase
    {
        public AirdropAlreadyClaimException() : base($"Airdrop already claimed")
        {
        }

        public override string Code => "AIRDROP_ALREADY_CLAIMED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}