using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Mosaico.API.Base.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClaimValue(this HttpContext context, string type)
        {
            var name = context.User.Claims.FirstOrDefault(c => c.Type == type);
            return name?.Value;
        }
    }
}