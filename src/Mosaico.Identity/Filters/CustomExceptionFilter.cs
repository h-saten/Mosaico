using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Authorization.Base.Configurations;
using Mosaico.Base.Exceptions;
using Mosaico.Identity.Configurations;
using Mosaico.Identity.Infrastructure;
using Mosaico.Validation.Base.Exceptions;
using Serilog;

namespace Mosaico.Identity.Filters
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IdentityServiceConfiguration _serviceConfiguration;
        private readonly ExternalProvidersConfiguration _externalProvidersConfiguration;
        public CustomExceptionFilterAttribute(IdentityServiceConfiguration serviceConfiguration, ExternalProvidersConfiguration externalProvidersConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
            _externalProvidersConfiguration = externalProvidersConfiguration;
        }
        
        public override void OnException(ExceptionContext context)
        {
            ApiResponse apiResponse = null;
            
            if (context.Exception is ExternalLoginException externalLoginException)
            {
                var baseUri = _serviceConfiguration.BaseUri;
                var errorUrl = _externalProvidersConfiguration.ErrorRedirectUrl;
                var uriBuilder = new UriBuilder($"{baseUri}{errorUrl}");
                var parameters = HttpUtility.ParseQueryString(string.Empty);
                if (externalLoginException?.Provider != null) parameters["provider"] = externalLoginException.Provider;
                if (externalLoginException?.Error != null) parameters["error"] = externalLoginException.Error;
                uriBuilder.Query = parameters.ToString() ?? string.Empty;
                var finalUrl = uriBuilder.Uri.ToString();
                context.Result = new RedirectResult(finalUrl, true);
                context.Result.ExecuteResultAsync(context);
                return;
            }
            
            if (context.Exception is ApiException apiException)
            {
                context.Exception = null;
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.StatusCode = apiException.StatusCode;
                context.HttpContext.Response.ContentType = apiException.ContentType;
                apiResponse = new ApiResponse(apiException.Message);
            }
            else if (context.Exception is ValidationException validationException)
            {
                context.Exception = null;
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.StatusCode = validationException.StatusCode;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new ErrorResult(validationException); 
                base.OnException(context);
                return;
            }
            else if (context.Exception is ExceptionBase exceptionBase)
            {
                context.Exception = null;
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.StatusCode = exceptionBase.StatusCode;
                context.HttpContext.Response.ContentType = "application/json";
                apiResponse = new ApiResponse(exceptionBase.Message);
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiResponse = new ApiResponse("Unauthorized Access");
                context.HttpContext.Response.StatusCode = 401;
                // handle logging here
            }
            else
            {
                // Unhandled errors
#if !DEBUG
                var msg = "An unhandled error occurred.";   
                apiResponse = new ApiResponse(msg);
                string stack = null;
#else
                var msg = context.Exception.GetBaseException().Message;
                apiResponse = new ApiResponse(msg);
                
                string stack = context.Exception.StackTrace;
                apiResponse.Detail = stack;
#endif
                
                context.HttpContext.Response.StatusCode = 400;

                // handle logging here
            }

            // always return a JSON result
            context.Result = new JsonResult(apiResponse);

            base.OnException(context);
        }

    }
}
