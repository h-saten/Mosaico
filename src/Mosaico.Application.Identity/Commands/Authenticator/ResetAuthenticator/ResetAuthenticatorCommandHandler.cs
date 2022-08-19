using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Application.Identity.Commands.Authenticator.ResetAuthenticator
{
    public class ResetAuthenticatorCommandHandler : IRequestHandler<ResetAuthenticatorCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityContext _identityContext;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ResetAuthenticatorCommandHandler(UserManager<ApplicationUser> userManager, IIdentityContext identityContext, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _signInManager = signInManager;
        }

        public async Task<Unit> Handle(ResetAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken);
            if (user == null) throw new UserNotFoundException(request.UserId);
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            return Unit.Value;
        }
    }
}