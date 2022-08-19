using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class StageAlreadyExistsException : ExceptionBase
    {
        public StageAlreadyExistsException(string name) : base($"Stage with name {name} already exists")
        {
        }

        public override string Code => "STAGE_ALREADY_EXISTS";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}