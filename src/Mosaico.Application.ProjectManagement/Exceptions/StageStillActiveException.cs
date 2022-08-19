using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class StageStillActiveException : ExceptionBase
    {
        public StageStillActiveException(string id, string activeStage) : base($"Stage {id} conflicts with other stage which is still active")
        {
        }

        public override string Code => Constants.ErrorCodes.AnotherStageIsStillActive;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}