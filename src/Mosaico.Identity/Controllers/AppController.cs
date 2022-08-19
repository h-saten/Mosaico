using System.Linq;
using System.Threading.Tasks;
using KangaExchange.SDK.Configurations;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Authorization.Base.Configurations;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Identity.Configurations;
using Mosaico.Identity.Models;

namespace Mosaico.Identity.Controllers
{
    [Authorize]
    [Route("api/configurations")]
    [ApiController]
    public class AppController : Controller
    {
        private readonly IdentityServiceConfiguration _configuration;
        private readonly KangaConfiguration _kangaConfiguration;
        private readonly ExternalProvidersConfiguration _externalProvidersConfiguration;

        public AppController(
            IdentityServiceConfiguration configuration, 
            KangaConfiguration kangaConfiguration, 
            ExternalProvidersConfiguration externalProvidersConfiguration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _kangaConfiguration = kangaConfiguration;
            _externalProvidersConfiguration = externalProvidersConfiguration;
        }

        [HttpGet]
        [AllowAnonymous]
        public Task<IActionResult> Configuration()
        {
            var config = new AppConfigurationDTO
            {
                MainUrl = _configuration.AfterLoginRedirectUrl,
                KangaAppId = _kangaConfiguration.Api?.AppId,
                KangaBaseUrl = _kangaConfiguration.BaseUrl,
                AfterLogoutUrl = _configuration.PostLogoutRedirectUris?.FirstOrDefault(),
                GoogleAuthenticationEnabled = _externalProvidersConfiguration.Google.IsEnabled,
                FacebookAuthenticationEnabled = _externalProvidersConfiguration.Facebook.IsEnabled,
                KangaAuthenticationEnabled = _kangaConfiguration.IsEnabled,
                RecaptchaSiteKey = _configuration.RecaptchaSiteKey,
                GatewayUrl = _configuration.GatewayUrl
            };

            return Task.FromResult((IActionResult)new SuccessResult(config));
        }
    }
}
