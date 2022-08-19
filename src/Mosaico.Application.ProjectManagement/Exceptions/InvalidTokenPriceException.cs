using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class InvalidTokenPriceException : ExceptionBase
    {
        public InvalidTokenPriceException(string message) : base($"{message} invalid token price")
        {
        }

        public override string Code => "INVALID_TOKEN_PRICE";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}