using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ProjectMemberLimitExceededException : ExceptionBase
    {
        public ProjectMemberLimitExceededException() : base($"Project member limit exceeded. Current limit is {Constants.ProjectMemberLimit}")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectMemberLimitExceeded;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}