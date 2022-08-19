using System;

namespace Mosaico.API.Base.Exceptions
{
    public class ExternalLoginException : Exception
    {

        public string Provider { get; private set; }
        public string Error { get; private set; }

        private ExternalLoginException()
        {

        }

        public ExternalLoginException(string provider, string message)
            : base($"{provider} login error: {message}")
        {
            Provider = provider;
            Error = message;
        }
    }
}