using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class CannotDeleteProjectCreatorException : ExceptionBase
    {
        public CannotDeleteProjectCreatorException() : base($"Cannot remove project creator")
        {
        }

        public override string Code => Constants.ErrorCodes.CannotRemoveProjectCreator;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}