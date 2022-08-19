using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Statistics.Exceptions
{
    public class ProjectNotFoundException : ExceptionBase
    {
        public Guid Id;
        
        public ProjectNotFoundException(Guid id) : base($"Project: '{id}' not found.")
        {
            Id = id;
        }

        public override string Code => Constants.ErrorCodes.ProjectNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}