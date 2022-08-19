using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Authorization.Base.Exceptions
{
    public class UnauthorizedException : ExceptionBase
    {
        public UnauthorizedException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.Unauthorized;
        public override int StatusCode => StatusCodes.Status401Unauthorized;
    }
}