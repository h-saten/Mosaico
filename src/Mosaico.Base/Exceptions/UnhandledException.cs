using Microsoft.AspNetCore.Http;

namespace Mosaico.Base.Exceptions
{
    public class UnhandledException : ExceptionBase
    {
        public UnhandledException(string message) : base(message)
        {
        }

        public override string Code => "UNHANDLED_EXCEPTION";
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}