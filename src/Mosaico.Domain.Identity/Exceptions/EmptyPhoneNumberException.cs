using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Identity.Exceptions
{
    public class EmptyPhoneNumberException : ExceptionBase
    {
        
        public EmptyPhoneNumberException() : base($"Lack of phone number.")
        {}

        public override string Code => Constants.ErrorCodes.EmptyPhoneNumber;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}