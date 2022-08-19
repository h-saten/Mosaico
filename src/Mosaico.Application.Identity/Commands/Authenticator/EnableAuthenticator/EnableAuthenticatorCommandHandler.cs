using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Identity.Commands.Authenticator.EnableAuthenticator
{
    public class EnableAuthenticatorCommandHandler : IRequestHandler<EnableAuthenticatorCommand, EnableAuthenticatorCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityContext _identityContext;

        public EnableAuthenticatorCommandHandler(UserManager<ApplicationUser> userManager, IIdentityContext identityContext)
        {
            _userManager = userManager;
            _identityContext = identityContext;
        }

        public async Task<EnableAuthenticatorCommandResponse> Handle(EnableAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken);
            if (user == null) throw new UserNotFoundException(request.UserId);
            var verificationCode = request.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);
            if (!is2faTokenValid)
            {
                throw new InvalidMFATokenException();
            }
            
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            if (await _userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                return new EnableAuthenticatorCommandResponse
                {
                    RecoveryCodes = recoveryCodes.ToList()
                };
            }

            return new EnableAuthenticatorCommandResponse();
        }
    }
}