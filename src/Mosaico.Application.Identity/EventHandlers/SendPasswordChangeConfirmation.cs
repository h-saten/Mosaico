using System.Text;
using System.Threading.Tasks;
using System.Web;
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
    [EventInfo(nameof(SendPasswordChangeConfirmation), "users:id")]
    [EventTypeFilter(typeof(UserConfirmedPasswordChange))]
    public class SendPasswordChangeConfirmation : EventHandlerBase
    {
        private readonly IIdentityEmailService _emailService;
        private readonly ILogger _logger;
        private readonly IIndex<string, string> _urls;
        private readonly UserManager<ApplicationUser> _userManager;

        public SendPasswordChangeConfirmation(IIdentityEmailService emailService,
            UserManager<ApplicationUser> userManager, IIndex<string, string> urls, ILogger logger = null)
        {
            _emailService = emailService;
            _userManager = userManager;
            _urls = urls;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserConfirmedPasswordChange>();

            if (userEvent != null)
            {
                _logger?.Verbose($"Initiating sending email for password change confirmation to the user {userEvent.Id}");
                var user = await _userManager.FindByIdAsync(userEvent.Id.ToString());
                if (user == null) throw new UserNotFoundException(userEvent.Id);
                var baseUri = "/";
                if (_urls.TryGetValue(Constants.UrlKeys.BaseUri, out var fetchedBaseUri))
                {
                    baseUri = fetchedBaseUri;
                }

                var callbackUrl = $"{baseUri}/auth/reportStolen?userId={userEvent.Id}&code={userEvent.code}";

                await _emailService.SendPasswordChangedNotificationAsync(userEvent.email,callbackUrl, user.Language);
            }
        }
    }
}