using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class FaqNotFoundException : ExceptionBase
    {
        public FaqNotFoundException(string id) : base($"FAQ {id} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.FaqNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}