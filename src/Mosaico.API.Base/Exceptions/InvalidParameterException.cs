using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.API.Base.Exceptions
{
    public class InvalidParameterException : ExceptionBase
    {
        public InvalidParameterException(string parameterName, string value = null) : base(
            $"Invalid parameter {parameterName}. Unexpected value {value}")
        {
            ExtraData = new
            {
                parameterName,
                value
            };
        }

        public override string Code => Constants.ErrorCodes.InvalidParameters;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}