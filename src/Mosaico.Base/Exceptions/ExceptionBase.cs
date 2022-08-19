using System;

namespace Mosaico.Base.Exceptions
{
    /*
     * Base exception class for all further custom exceptions within all modules and integrations
     */
    public abstract class ExceptionBase : Exception
    {
        protected ExceptionBase(string message) : base(message)
        {
        }

        protected ExceptionBase(string message, object extraData) : base(message)
        {
            ExtraData = extraData;
        }

        protected ExceptionBase(string message, object extraData, Exception innerException) : base(message,
            innerException)
        {
            ExtraData = extraData;
        }

        /*
         * Any extra data that might be helpful for consumers (other systems, web UI etc)
         */
        public object ExtraData { get; protected set; }

        /*
         * centrally assigned error code for tracing and localization
         */
        public abstract string Code { get; }

        /*
         * status code to be return in controller
         */
        public abstract int StatusCode { get; }
    }
}