using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.BackgroundJobs.Hangfire.Exceptions
{
    public class InvalidHangfireJobException : ExceptionBase
    {
        public InvalidHangfireJobException(string job) : base($"Job {job} was not valid to run as hangfire job")
        {
        }

        public override string Code => Constants.ErrorCodes.INVALID_HANGFIRE_JOB;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}