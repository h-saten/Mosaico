using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Identity.Exceptions
{
    public class UserNotEntitledException: ExceptionBase
    {
        public UserNotEntitledException(string id) : base($"User of ID {id} does not possess relevant permissions to proceed further")
        {
        }

        public override string Code => Constants.ErrorCodes.LackOfPermissions;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}