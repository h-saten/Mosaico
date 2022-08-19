using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Identity.Exceptions
{
    public class PhoneNumberAlreadyConfirmedException : ExceptionBase
    {
        public PhoneNumberAlreadyConfirmedException(string userId) : base($"User {userId} has already confirmed phone number")
        {
        }

        public override string Code => "PHONE_NUMBER_ALREADY_CONFIRMED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}