using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class StageNotClosedException : ExceptionBase
    {
        public StageNotClosedException(string id) : base($"Stage {id} is not closed")
        {
        }

        public override string Code => Constants.ErrorCodes.StageNotClosed;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}