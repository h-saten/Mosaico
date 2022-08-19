using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Application.Identity.Exceptions
{
    public class InvalidPhoneNumberConfirmationCodeException : ExceptionBase
    {
        public PhoneNumber TelephoneNumber { get; set; }
        public string ConfirmationCode { get; set; }
        
        public InvalidPhoneNumberConfirmationCodeException(PhoneNumber value, string code) : base($"Confirmation code: '{code}' for phone number: {value} is invalid.")
        {
            TelephoneNumber = value;
            ConfirmationCode = code;
        }

        public override string Code => Constants.ErrorCodes.InvalidPhoneNumberConfirmationCode;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}