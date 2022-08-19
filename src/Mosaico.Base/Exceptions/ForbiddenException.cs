using Microsoft.AspNetCore.Http;

namespace Mosaico.Base.Exceptions
{
    public class ForbiddenException : ExceptionBase
    {
        public ForbiddenException(string message) : base(message)
        {
        }
        
        public ForbiddenException() : base("Forbidden")
        {
        }

        public override string Code => Constants.ExceptionCodes.Forbidden;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}