using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class PageNotFoundException : ExceptionBase
    {
        public PageNotFoundException(string id) : base($"Page {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.PageNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}