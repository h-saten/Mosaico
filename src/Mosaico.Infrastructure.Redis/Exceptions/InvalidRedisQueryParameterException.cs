using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Infrastructure.Redis.Exceptions
{
    public class InvalidRedisQueryParameterException : ExceptionBase
    {
        public InvalidRedisQueryParameterException(string parameterName) : base($"Parameter {parameterName} is missing")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidRedisQueryParameters;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}