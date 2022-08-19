using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class InvalidStageIdException : ExceptionBase
    {
        public InvalidStageIdException() : base($"Received invalid stage id")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidStageId;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}