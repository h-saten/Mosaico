using Microsoft.AspNetCore.Http;

namespace Mosaico.Base.Exceptions
{
    public class RecaptchaException : ExceptionBase
    {
        public RecaptchaException(string message) : base(message)
        {
        }

        public override string Code => Constants.ExceptionCodes.InvalidCaptcha;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}