using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Identity.Exceptions
{
    public class UserAccountDeletionJobException : ExceptionBase
    {
        public UserAccountDeletionJobException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.UserAccountDeletionJobException;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}