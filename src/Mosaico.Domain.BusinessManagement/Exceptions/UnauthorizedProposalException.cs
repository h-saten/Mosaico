using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class UnauthorizedProposalException : ExceptionBase
    {
        public UnauthorizedProposalException() : base($"Unauthorized proposal")
        {
        }

        public override string Code => Constants.ErrorCodes.UnauthorizedProposal;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}