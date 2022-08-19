using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.UserCom.Exceptions
{
    public class UserComException : ExceptionBase
    {
        public UserComException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.ConnectionError;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}