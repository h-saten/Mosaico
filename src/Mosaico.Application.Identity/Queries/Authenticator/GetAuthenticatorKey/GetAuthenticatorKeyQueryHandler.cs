using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Authorization.Base.Configurations;
using Mosaico.Authorization.Base.Exceptions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Application.Identity.Queries.Authenticator.GetAuthenticatorKey
{
    public class GetAuthenticatorKeyQueryHandler : IRequestHandler<GetAuthenticatorKeyQuery, GetAuthenticatorKeyQueryResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthenticatorConfiguration _configuration;
        private readonly IIdentityContext _identityContext;

        public GetAuthenticatorKeyQueryHandler(UserManager<ApplicationUser> userManager, IIdentityContext identityContext, AuthenticatorConfiguration configuration)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _configuration = configuration;
        }

        public async Task<GetAuthenticatorKeyQueryResponse> Handle(GetAuthenticatorKeyQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken);
            if (user == null || user.IsDeactivated) throw new UserNotFoundException(request.UserId);

            if (!_configuration.IsEnabled) throw new AuthenticatorDisabledException();
            
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var sharedKey = unformattedKey.FormatAsOTPKey();

            var email = await _userManager.GetEmailAsync(user);
            var authenticationUri = string.Format(
                _configuration.AuthenticatorUriFormat,
                UrlEncoder.Default.Encode(_configuration.OtpDomain),
                UrlEncoder.Default.Encode(email),
                unformattedKey);
                
            return new GetAuthenticatorKeyQueryResponse
            {
                SharedKey = sharedKey,
                AuthenticationUri = authenticationUri
            };
        }
    }
}