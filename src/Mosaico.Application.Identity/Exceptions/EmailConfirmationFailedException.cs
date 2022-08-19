using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Identity.Exceptions
{
    public class EmailConfirmationFailedException : ExceptionBase
    {
        public EmailConfirmationFailedException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.EmailConfirmationFailed;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}