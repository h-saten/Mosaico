using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Identity.Exceptions
{
    public class InvalidPhoneNumberException : ExceptionBase
    {
        public string PhoneNumber { get; set; }
        
        public InvalidPhoneNumberException(string phoneNumber) : base($"Phone number: '{phoneNumber}' is invalid.")
        {
            PhoneNumber = phoneNumber;
        }

        public override string Code => Constants.ErrorCodes.EmptyPhoneNumber;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}