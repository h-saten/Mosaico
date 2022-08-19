using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Identity.Exceptions
{
    public class InvalidConfirmationCodeException : ExceptionBase
    {
        public string ConfirmationCode { get; set; }
        
        public InvalidConfirmationCodeException(string value) : base($"Code: '{value}' is invalid.")
        {
            ConfirmationCode = value;
        }

        public override string Code => Constants.ErrorCodes.PermissionNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}