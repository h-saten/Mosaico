using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Identity.Exceptions
{
    public class PermissionNotFoundException : ExceptionBase
    {
        public PermissionNotFoundException(string key) : base($"Permission with {key} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.PermissionNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}