using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class StageEndDateException : ExceptionBase
    {
        public StageEndDateException(string stageId) : base($"Stage {stageId} cannot start without an end date")
        {
        }

        public override string Code => Constants.ErrorCodes.StageCannotStart;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}