using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Application.Identity.Exceptions
{
    public class ConfirmationCodeCannotBeGeneratedException : ExceptionBase
    {
        public PhoneNumber TelephoneNumber { get; set; }
        
        public ConfirmationCodeCannotBeGeneratedException(PhoneNumber value) : base($"Confirmation code cannot be generated for number: {value}")
        {
            TelephoneNumber = value;
        }

        public override string Code => Constants.ErrorCodes.ConfirmationCodeGenerationFailed;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}