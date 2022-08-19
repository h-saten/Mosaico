using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class CrowdsaleAlreadyDeployedException : ExceptionBase
    {
        public CrowdsaleAlreadyDeployedException() : base($"Crowdsale already deployed")
        {
        }

        public override string Code => "CROWDSALE_ALREADY_DEPLOYED";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}