using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class TemplateNotFoundException : ExceptionBase
    {
        
        public TemplateNotFoundException(string key) : base($"Template with key {key} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.TemplateNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}