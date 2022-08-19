using System;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.EventHandlers
{
    [EventInfo(nameof(SendRegisterConfirmationEmailOnExternalUserCreated),  "users:id")]
    [EventTypeFilter(typeof(ExternalUserCreatedEvent))]
    public class SendRegisterConfirmationEmailOnExternalUserCreated : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IIdentityContext _identityContext;
        private readonly IIdentityEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIndex<string, string> _urls;

        public SendRegisterConfirmationEmailOnExternalUserCreated(IIdentityContext identityContext, IIdentityEmailService emailService, UserManager<ApplicationUser> userManager, IIndex<string, string> urls, ILogger logger = null)
        {
            _identityContext = identityContext;
            _emailService = emailService;
            _userManager = userManager;
            _urls = urls;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserCreatedEvent>();
            if (userEvent != null)
            {
                var userId = userEvent.Id.ToString();
                var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    var baseUri = "/";
                    if (_urls.TryGetValue(Constants.UrlKeys.BaseUri, out var fetchedBaseUri))
                    {
                        baseUri = fetchedBaseUri;
                    }
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var tokenGeneratedBytes = Encoding.UTF8.GetBytes(code);
                    var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                    var callbackUrl = $"{baseUri}/auth/confirmEmail?userId={user.Id}&code={codeEncoded}";
                    await _emailService.SendExternalUserConfirmationEmailAsync(user, callbackUrl, user.Language);
                }
                else
                    _logger?.Warning($"User ${userEvent.Id} was not found in database");
            }
        }
    }
}