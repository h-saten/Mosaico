using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class CannotSelfDeleteException : ExceptionBase
    {
        public CannotSelfDeleteException() : base($"Cannot delete yourself")
        {
        }

        public override string Code => Constants.ErrorCodes.CannotSelfDelete;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}