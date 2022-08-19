using System;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.EventHandlers
{
    [EventInfo(nameof(SendForgotPasswordEmailOnResetInitiated),  "users:id")]
    [EventTypeFilter(typeof(UserInitiatedPasswordReset))]
    public class SendForgotPasswordEmailOnResetInitiated : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IIdentityEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIndex<string, string> _urls;

        public SendForgotPasswordEmailOnResetInitiated(IIdentityEmailService emailService, UserManager<ApplicationUser> userManager, IIndex<string, string> urls, ILogger logger = null)
        {
            _emailService = emailService;
            _userManager = userManager;
            _urls = urls;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserInitiatedPasswordReset>();
            if (userEvent != null)
            {
                var user = await _userManager.FindByIdAsync(userEvent.Id.ToString());
                if (user == null)
                {
                    throw new UserNotFoundException(userEvent.Id);
                }
                
                var baseUri = "/";
                if (_urls.TryGetValue(Constants.UrlKeys.BaseUri, out var fetchedBaseUri))
                {
                    baseUri = fetchedBaseUri;
                }
                
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var tokenGeneratedBytes = Encoding.UTF8.GetBytes(code);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var callbackUrl = $"{baseUri}/auth/resetPassword?userId={user.Id}&code={codeEncoded}";
                await _emailService.SendForgotPasswordEmailAsync(user, callbackUrl, user.Language);
            }
        }
    }
}