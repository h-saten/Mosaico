using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Serilog;

namespace Mosaico.Application.Identity.Commands.ResetUserPassword
{
    public class ResetUserPasswordCommandHandler : IRequestHandler<ResetUserPasswordCommand>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityContext _identityContext;

        public ResetUserPasswordCommandHandler(UserManager<ApplicationUser> userManager, IIdentityContext identityContext, ILogger logger = null)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user != null) 
            {
                _logger?.Verbose($"User {request.Email} was found. Attempting to set new password");
                var codeDecodedBytes = WebEncoders.Base64UrlDecode(request.Code);
                var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
                var resultResetPassword = await _userManager.ResetPasswordAsync(user, codeDecoded, request.Password);
                var resetResult = await _userManager.ResetAccessFailedCountAsync(user);
                var resetAuthKey = await _userManager.ResetAuthenticatorKeyAsync(user);
                var resultLockout = await _userManager.SetLockoutEnabledAsync(user, false);
                var resultUpdateSecurityStamp = await _userManager.UpdateSecurityStampAsync(user);

                if (
                    resultResetPassword.Succeeded && 
                    resetResult.Succeeded && 
                    resetAuthKey.Succeeded && 
                    resultLockout.Succeeded && 
                    resultUpdateSecurityStamp.Succeeded
                ) {
                    var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                    if (!emailConfirmed)
                    {
                        user.EmailConfirmed = true;
                        await _identityContext.SaveChangesAsync(cancellationToken);
                    }
                }
                else
                {
                    _logger?.Verbose($"Something went wrong during password reset");
                    _logger?.Verbose(resultResetPassword);
                    _logger?.Verbose(resetResult);
                    _logger?.Verbose(resetAuthKey);
                    _logger?.Verbose(resultLockout);
                    _logger?.Verbose(resultUpdateSecurityStamp);
                }
            }
            return Unit.Value;
        }
    }
}