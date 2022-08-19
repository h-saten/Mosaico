using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Airdrop
{
    public class ForbiddenToClaimException : ExceptionBase
    {
        public ForbiddenToClaimException() : base("Forbidden to claim")
        {
        }

        public override string Code => "FORBIDDEN_TO_CLAIM_AIRDROP";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}