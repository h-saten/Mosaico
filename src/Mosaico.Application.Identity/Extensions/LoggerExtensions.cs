using System.Linq;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Extensions
{
    public static class LoggerExtensions
    {
        public static void Verbose(this ILogger logger, IdentityResult result)
        {
            if (result != null && result.Errors != null && result.Errors.Any() && logger != null)
            {
                foreach (var resultError in result.Errors)
                {
                    logger.Verbose($"{resultError.Code}: {resultError.Description}");
                }
            }
        }
    }
}