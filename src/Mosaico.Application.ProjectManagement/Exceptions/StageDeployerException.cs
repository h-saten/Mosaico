using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class StageDeployerException : ExceptionBase
    {
        public StageDeployerException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.StageDeployerError;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}