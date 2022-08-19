using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class StageStatusNotFoundException : ExceptionBase
    {
        public StageStatusNotFoundException(string title) : base($"Stage status {title} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.StageStatusNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}