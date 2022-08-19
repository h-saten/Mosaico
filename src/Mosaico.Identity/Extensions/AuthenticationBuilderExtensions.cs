using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using Mosaico.Authorization.Base.Configurations;

namespace Mosaico.Identity.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static void AddCustomGoogle(this AuthenticationBuilder authBuilder, ExternalProviderConfiguration provider, string errorUrl, string baseUri)
        {
            authBuilder?.AddGoogle(options =>
            {
                options.ClientId = provider.ClientId;
                options.ClientSecret = provider.ClientSecret;
                options.SaveTokens = true;
                options.AccessDeniedPath = errorUrl;
                options.Events = new OAuthEvents
                {
                    OnRemoteFailure = context =>
                    {
                        var uriBuilder = new UriBuilder($"{baseUri}{errorUrl}");
                        var parameters = HttpUtility.ParseQueryString(string.Empty);
                        parameters["provider"] = "Google";
                        parameters["error"] = context.Failure?.Message;
                        uriBuilder.Query = parameters.ToString() ?? string.Empty;
                        var finalUrl = uriBuilder.Uri.ToString();
                        context.Response.Redirect(finalUrl);
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
            });
        }
        
        public static void AddCustomFacebook(this AuthenticationBuilder authBuilder, ExternalProviderConfiguration provider, string errorUrl, string baseUri)
        {
            authBuilder?.AddFacebook(options =>
            {
                options.ClientId = provider.ClientId;
                options.ClientSecret = provider.ClientSecret;
                options.Fields.Add("picture");
                options.Fields.Add("email");
                options.SaveTokens = true;
                options.ClaimActions.MapJsonKey("picture", "picture");
                options.AccessDeniedPath = errorUrl;
                options.Events = new OAuthEvents
                {
                    OnRemoteFailure = context =>
                    {
                        var uriBuilder = new UriBuilder($"{baseUri}{errorUrl}");
                        var parameters = HttpUtility.ParseQueryString(string.Empty);
                        parameters["provider"] = "Facebook";
                        parameters["error"] = context.Failure?.Message;
                        uriBuilder.Query = parameters.ToString() ?? string.Empty;
                        var finalUrl = uriBuilder.Uri.ToString();
                        context.Response.Redirect(finalUrl);
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}