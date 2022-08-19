using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer;
using KangaExchange.SDK.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Identity.Queries.GetKangaUser;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.API.v1.Identity.Controllers
{
    [Route("api/auth/[controller]/[action]")]
    [ApiController]
    public class KangaController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityContext _context;
        private readonly IKangaAuthAPIClient _authApiClient;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public KangaController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityContext context, 
            IKangaAuthAPIClient authApiClient, 
            ILogger logger, 
            IMediator mediator) {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _authApiClient = authApiClient;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<OkResult> Login([FromBody] string token)
        {
            var provider = "KangaExchange";
            
            var userData = await _authApiClient.CheckAsync(token);
            var userEmail = userData.Data.Email;

            var providerKey = userData.Data.AppUserId;

            var user = await _userManager.FindByEmailAsync(userEmail);
            
            if (user == null)
            {
                _logger.Information($"[KangaLogin] Create new user for email {userEmail}.");
                
                user = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    NormalizedEmail = userEmail.ToUpper(),
                    NormalizedUserName = userEmail.ToUpper(),
                    KangaUser = new KangaUser
                    {
                        Email = userEmail,
                        KangaAccountId = providerKey
                    }
                };
                
                var identityResult = await _userManager.CreateAsync(user);

                if (!identityResult.Succeeded)
                {
                    _logger.Information($"[KangaLogin] Create account for email {userEmail} failed.");
                    throw new ApiException(identityResult.Errors.First().Description);
                }
                
                identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerKey, provider));
                if (!identityResult.Succeeded)
                {
                    _logger.Information($"[KangaLogin] Error while add kanga login info for {userEmail}.");
                    throw new ApiException(identityResult.Errors.First().Description);
                }
            }
            else
            {
                var externalLogin = await _userManager.FindByLoginAsync(provider, providerKey);
                user.KangaUser ??= new KangaUser
                {
                    Email = userEmail
                };
                user.KangaUser.KangaAccountId = userData.Data.AppUserId;
                
                await _userManager.UpdateAsync(user);
                if (externalLogin == null)
                {
                    var identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerKey, provider));
                    if (!identityResult.Succeeded)
                    {
                        _logger.Information($"[KangaLogin] Error while add kanga login info for {userEmail}.");
                        throw new ApiException(identityResult.Errors.First().Description);
                    }
                    
                    _logger.Information($"[KangaLogin] Added kanga login info for existing user {userEmail}.");

                }
            }

            var canSignIn = await _signInManager.CanSignInAsync(user);
            var isLockedOut = await _userManager.IsLockedOutAsync(user);

            if (!canSignIn)
            {
                _logger.Information($"[KangaLogin] Account {userEmail} can sign in.");
                throw new ApiException("Cannot log in");
            }

            if (isLockedOut)
            {
                _logger.Information($"[KangaLogin] Login account {userEmail} failed, because locked sign in.");
                throw new ApiException("Locked out account");
            }
            
            await _signInManager.SignInAsync(user, true);

            user.UpdateLastLogin();
            await _context.SaveChangesAsync();
            
            return Ok();
        }
        
        [HttpGet("{id}")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetKangaUserQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> KangaUser([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidParameterException(nameof(id), id);
            var user = await _mediator.Send(new GetKangaUserQuery {Id = id});
            return new SuccessResult(user);
        }
    }
}