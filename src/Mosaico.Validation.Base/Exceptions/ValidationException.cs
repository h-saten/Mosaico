using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Validation.Base.Exceptions
{
    public class ValidationException : ExceptionBase
    {
        public ValidationException(string errorCode, int statusCode = StatusCodes.Status400BadRequest) : base(
            "Validation error")
        {
            Code = errorCode;
            StatusCode = statusCode;
        }

        public ValidationException(string errorCode, object extraData, int statusCode = StatusCodes.Status400BadRequest)
            : base("Validation error", extraData)
        {
            Code = errorCode;
            StatusCode = statusCode;
        }

        public ValidationException(string errorCode, object extraData, Exception innerException,
            int statusCode = StatusCodes.Status400BadRequest) : base("Validation error", extraData, innerException)
        {
            Code = errorCode;
            StatusCode = statusCode;
        }

        public override string Code { get; }
        public override int StatusCode { get; }
    }
}