using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class UserNotVerifiedException : ExceptionBase
    {
        public UserNotVerifiedException(string id) : base($"User {id} is not verified")
        {
        }

        public override string Code => "USER_NOT_VERIFIED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}