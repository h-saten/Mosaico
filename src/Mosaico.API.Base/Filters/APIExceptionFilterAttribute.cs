using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Mosaico.API.Base.Responses;
using Mosaico.Base.Exceptions;
using Serilog;

namespace Mosaico.API.Base.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class APIExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public ILogger Logger;

        public APIExceptionFilterAttribute()
        {
            Logger = Log.Logger;
        }

        public override void OnException(ExceptionContext context)
        {
            Logger ??= Log.Logger;
            if (context.Exception is ExceptionBase exception)
            {
                Logger?.Warning(exception, "Handled exception");
                context.HttpContext.Response.StatusCode = exception.StatusCode;
                context.Result = new ErrorResult(exception);
            }
            else
            {
                Logger?.Error(context.Exception, "Unhandled exception");
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Result = new ErrorResult(Mosaico.Base.Constants.ExceptionCodes.UnhandledError,
                    context.Exception.Message, context.Exception.InnerException);
            }
        }
    }
}