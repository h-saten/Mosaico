using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Mosaico.Domain.Identity.Entities;
using Serilog;

namespace Mosaico.Application.Identity.Commands.LogoutUser
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, LogoutUserCommandResponse>
    {
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IPersistedGrantService _persistedGrantService;

        public LogoutUserCommandHandler(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor contextAccessor, IIdentityServerInteractionService interaction, IPersistedGrantService persistedGrantService, ILogger logger = null)
        {
            _logger = logger;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _interaction = interaction;
            _persistedGrantService = persistedGrantService;
        }

        public async Task<LogoutUserCommandResponse> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            var context = _contextAccessor.HttpContext;
            if (context != null)
            {
                var idp = context.User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                var subjectId = context.User.Identity.GetSubjectId();
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    if (request.LogoutId == null)
                    {
                        // if there's no current logout context, we need to create one
                        // this captures necessary info from the current logged in user
                        // before we signout and redirect away to the external IdP for signout
                        request.LogoutId = await _interaction.CreateLogoutContextAsync();
                    }
                }

                // delete authentication cookie
                await _signInManager.SignOutAsync();

                // set this so UI rendering sees an anonymous user
                context.User = new ClaimsPrincipal(new ClaimsIdentity());

                // get context information (client name, post logout redirect URI and iframe for federated signout)
                var logout = await _interaction.GetLogoutContextAsync(request.LogoutId);

                await _persistedGrantService.RemoveAllGrantsAsync(subjectId, "spa-tokenizer");
                
                return new LogoutUserCommandResponse
                {
                    PostLogoutRedirectUri = logout?.PostLogoutRedirectUri ?? request.ReturnUrl,
                    ClientName = logout?.ClientId,
                    SignOutIframeUrl = logout?.SignOutIFrameUrl
                };
            }

            return null;
        }
    }
}