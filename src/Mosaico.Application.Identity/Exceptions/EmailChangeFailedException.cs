using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Identity.Exceptions
{
    public class EmailChangeFailedException : ExceptionBase
    {
        public EmailChangeFailedException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.EmailChangeFailed;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}