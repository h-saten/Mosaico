using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class StageOverlappingException : ExceptionBase
    {
        public StageOverlappingException() : base($"There are stages with overlapping dates")
        {
        }

        public override string Code => Constants.ErrorCodes.StagesOverlap;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}