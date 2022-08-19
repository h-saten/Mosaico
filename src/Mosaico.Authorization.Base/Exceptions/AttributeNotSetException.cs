using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Authorization.Base.Exceptions
{
    public class AttributeNotSetException : ExceptionBase
    {
        public AttributeNotSetException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.AttributeNotSet;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}