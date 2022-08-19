using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class NotAllowedRedeploymentException : ExceptionBase
    {
        public NotAllowedRedeploymentException(string message) : base($"Stage {message} is not allowed to be redeployed")
        {
        }

        public override string Code => "NOT_ALLOWED_TO_REDEPLOY";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}