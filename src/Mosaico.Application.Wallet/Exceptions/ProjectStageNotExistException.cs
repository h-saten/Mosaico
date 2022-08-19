using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class ProjectStageNotExistException : ExceptionBase
    {
        public ProjectStageNotExistException(Guid projectId) : base($"Sale stage for project {projectId} not exist")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectSaleStageNotExist;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}