using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Identity.Exceptions
{
    public class AMLAlreadyCompletedException : ExceptionBase
    {
        public AMLAlreadyCompletedException() : base($"User already completed KYC.")
        {
        }

        public override string Code => Constants.ErrorCodes.AMLCompleted;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}