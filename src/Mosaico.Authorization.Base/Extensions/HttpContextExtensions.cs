using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Mosaico.Authorization.Base.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClaimValue(this HttpContext context, string type)
        {
            var name = context.User.Claims.FirstOrDefault(c => c.Type == type);
            return name?.Value;
        }
        
        public static string GetLanguage(this HttpContext context)
        {
            var key = "applanguage";
            var language = context
                .Request
                .Headers
                .FirstOrDefault(x => x.Key.ToLower() == key)
                .Value
                .FirstOrDefault()?
                .ToLower();

            return Mosaico.Base.Constants.Languages.All.Contains(language)
                ? language
                : Mosaico.Base.Constants.Languages.English;
        }
    }
}