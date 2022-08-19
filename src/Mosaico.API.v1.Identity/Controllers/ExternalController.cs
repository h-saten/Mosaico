using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Filters;
using Mosaico.Domain.Identity.Entities;
using Serilog;
using Serilog.Events;

namespace Mosaico.API.v1.Identity.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IIndex<string, string> _urls;
        private readonly ILogger _logger;
        
        private const string DefaultRedirectUrl = "~/";

        public ExternalController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            IIndex<string, string> urls,
            ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
            _urls = urls;
            _logger = logger;
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult Challenge([FromQuery] string provider,[FromQuery] string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = DefaultRedirectUrl;

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new ExternalLoginException(provider, "invalid_return_url");
            }
            
            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)), 
                Items =
                {
                    { "returnUrl", returnUrl }, 
                    { "scheme", provider },
                }
            };

            return Challenge(props, provider);
            
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            try
            {
                // read external identity from the temporary cookie
                var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
                
                if (result?.Succeeded != true)
                {
                    var providerName = result?.Properties?.Items["scheme"];
                    throw new ExternalLoginException(providerName, "External authentication error");
                }

                if (_logger.IsEnabled(LogEventLevel.Debug) && result.Principal != null)
                {
                    var externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
                    _logger?.Debug("External claims: {@claims}", externalClaims);
                }

                // lookup our user and external provider info
                var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);

                if (user == null)
                {
                    // this might be where you might initiate a custom workflow for user registration
                    // in this sample we don't show how that would be done, as our sample implementation
                    // simply auto-provisions new external user
                    user = await AutoProvisionUserAsync(provider, providerUserId, claims);
                }

                if (user.IsDeactivated)
                {
                    _logger?.Verbose($"User {user.Email} deactivated");
                    await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                    return BadRequest("DEACTIVATED");
                }

                // this allows us to collect any additional claims or properties
                // for the specific protocols used and store them in the local auth cookie.
                // this is typically used to store data needed for signout from those protocols.
                var additionalLocalClaims = new List<Claim>();
                var localSignInProps = new AuthenticationProperties();
                ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);
                
                // issue authentication cookie for user
                // we must issue the cookie maually, and can't use the SignInManager because
                // it doesn't expose an API to issue additional claims from the login workflow
                var principal = await _signInManager.CreateUserPrincipalAsync(user);
                additionalLocalClaims.AddRange(principal.Claims);
                var name = principal.FindFirst(JwtClaimTypes.Name)?.Value ?? user.Id;
                
                var isuser = new IdentityServerUser(user.Id)
                {
                    DisplayName = name,
                    IdentityProvider = provider,
                    AdditionalClaims = additionalLocalClaims
                };

                await HttpContext.SignInAsync(isuser, localSignInProps);
                
                await ConfirmUserEmailIfNeededAsync(user);
                await UpdateLastLoginAsync(user);
                
                // delete temporary cookie used during external authentication
                await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
                
                // retrieve return URL
                _urls.TryGetValue(Application.Identity.Constants.UrlKeys.AfterLoginRedirectUrl, out var afterLoginRedirectUrl);
                var resultReturnUrl = result.Properties?.Items["returnUrl"];
                var returnUrl = resultReturnUrl is not null && resultReturnUrl != DefaultRedirectUrl
                    ? result.Properties?.Items["returnUrl"] : afterLoginRedirectUrl;

                // check if external login is in the context of an OIDC request
                var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
                await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, name, true, context?.Client.ClientId));

                return Redirect(returnUrl);
            }
            catch (ExternalLoginException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExternalLoginException(null, e.Message);
            }
        }

        private async Task<(ApplicationUser user, string provider, string providerUserId, IEnumerable<Claim> claims)>
            FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser?.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser?.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties?.Items["scheme"];
            var providerUserId = userIdClaim.Value;
            
            // email
            var emailClaim = externalUser.FindFirst(JwtClaimTypes.Email) ??
                             externalUser.FindFirst(ClaimTypes.Email) ??
                             throw new ExternalLoginException(provider, "email_required");

            // find external user
            var user = await _userManager.FindByLoginAsync(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        private async Task<ApplicationUser> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            // user's display name
            var name = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (name != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, name));
            }
            else
            {
                var first = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                var last = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                    claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
                if (first != null && last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
                }
                else if (first != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                }
                else if (last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            // email
            var email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
               claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Email, email));
            }
            
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var addExternalLogin = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
                if (!addExternalLogin.Succeeded) throw new ExternalLoginException(provider,addExternalLogin.Errors.First().Description);

                return user;
            }

            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                NormalizedEmail = email.ToUpperInvariant(),
                NormalizedUserName = email.ToUpperInvariant()
            };
            var identityResult = await _userManager.CreateAsync(user);
            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

            if (filtered.Any())
            {
                identityResult = await _userManager.AddClaimsAsync(user, filtered);
                if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
            }

            identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

            return user;
        }

        private async Task ConfirmUserEmailIfNeededAsync(ApplicationUser user)
        {
            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }
        }

        private async Task UpdateLastLoginAsync(ApplicationUser user)
        {
            user.UpdateLastLogin();
            await _userManager.UpdateAsync(user);
        }

        // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
        // this will be different for WS-Fed, SAML2p or other protocols
        private void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal?.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var idToken = externalResult.Properties?.GetTokenValue("id_token");
            if (idToken != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
            }
        }
    }
}