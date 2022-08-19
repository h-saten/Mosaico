using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace KangaExchange.SDK.Exceptions
{
    public class KangaException : ExceptionBase
    {
        public KangaException(string errorCode, string message) : base(message, new { errorCode })
        {
        }
        
        public KangaException(int errorCode, string message) : base(message, new { errorCode })
        {
        }
        
        public KangaException(string message) : base(message, new { errorCode = "0000" })
        {
        }

        public override string Code => Constants.ErrorCodes.KangaException;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}