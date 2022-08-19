using Microsoft.AspNetCore.Http;

namespace Mosaico.Base.Exceptions
{
    public class LimitExceededException : ExceptionBase
    {
        public LimitExceededException(string entity) : base($"Limit of {entity} exceeded.")
        {
        }

        public override string Code => Constants.ExceptionCodes.LimitExceeded;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}