using System;

namespace Mosaico.SDK.Base.Exceptions
{
    public sealed class MosaicoException : Exception
    {
        public MosaicoException(string message, string code = Constants.ErrorCodes.UnhandledError, int statusCode = Constants.DefaultErrorStatusCode, object extraData = null) : base(message)
        {
            StatusCode = statusCode;
            Code = code;
            ExtraData = extraData;
        }

        public string Code { get; set; }
        
        public int StatusCode { get; set; }
        
        public object ExtraData { get; set; }
    }
}