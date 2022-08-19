using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class FaqLimitExceededException : ExceptionBase
    {
        public FaqLimitExceededException(string pageId) : base($"FAQ limit exceeded for page {pageId}")
        {
        }

        public override string Code => Constants.ErrorCodes.FaqLimitExceeded;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}