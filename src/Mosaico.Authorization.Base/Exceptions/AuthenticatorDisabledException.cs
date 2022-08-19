using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Authorization.Base.Exceptions
{
    public class AuthenticatorDisabledException : ExceptionBase
    {
        public AuthenticatorDisabledException() : base($"Authenticator disabled")
        {
        }

        public override string Code => "AUTHENTICATOR_DISABLED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}