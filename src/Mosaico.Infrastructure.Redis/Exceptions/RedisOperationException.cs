using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Infrastructure.Redis.Exceptions
{
    public class RedisOperationException : ExceptionBase
    {
        public RedisOperationException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.RedisOperationFailed;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}