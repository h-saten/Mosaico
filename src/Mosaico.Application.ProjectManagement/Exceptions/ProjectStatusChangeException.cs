using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ProjectStatusChangeException : ExceptionBase
    {
        public ProjectStatusChangeException(string projectStatus, string projectId) : base($"Unable to change project {projectId} status to {projectStatus}")
        {
        }

        public override string Code => Constants.ErrorCodes.UnableToChangeProjectStatus;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}