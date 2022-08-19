using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class StageNotFoundException : ExceptionBase
    {
        public StageNotFoundException(Guid projectId, Guid id) : base($"Stage with id {id} was not found for project {projectId}")
        {
        }

        public StageNotFoundException(Guid id): base($"Stage with id {id} was not found")
        {
            
        }
        
        public override string Code => Constants.ErrorCodes.StageNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}