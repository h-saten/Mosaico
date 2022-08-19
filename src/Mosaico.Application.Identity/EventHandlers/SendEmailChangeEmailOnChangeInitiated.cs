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
    [EventInfo(nameof(SendEmailChangeEmailOnChangeInitiated), "users:id")]
    [EventTypeFilter(typeof(UserInitiatedEmailChange))]
    public class SendEmailChangeEmailOnChangeInitiated : EventHandlerBase
    {
        private readonly IIdentityEmailService _emailService;
        private readonly ILogger _logger;
        private readonly IIndex<string, string> _urls;
        private readonly UserManager<ApplicationUser> _userManager;

        public SendEmailChangeEmailOnChangeInitiated(IIdentityEmailService emailService,
            UserManager<ApplicationUser> userManager, IIndex<string, string> urls, ILogger logger = null)
        {
            _emailService = emailService;
            _userManager = userManager;
            _urls = urls;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserInitiatedEmailChange>();

            if (userEvent != null)
            {
                _logger?.Verbose($"Initiating change of the email for user {userEvent.Id}");
                var user = await _userManager.FindByIdAsync(userEvent.Id.ToString());
                if (user == null) throw new UserNotFoundException(userEvent.Id);
                var baseUri = "/";
                if (_urls.TryGetValue(Constants.UrlKeys.BaseUri, out var fetchedBaseUri)) 
                    baseUri = fetchedBaseUri;

                var code = await _userManager.GenerateChangeEmailTokenAsync(user, userEvent.Email);
                var codeEncoded = HttpUtility.UrlEncode(code);
                var callbackUrl = $"{baseUri}/auth/changeEmail?userId={user.Id}&email={userEvent.Email}&code={codeEncoded}";
                await _emailService.SendEmailChangeEmailAsync(callbackUrl, userEvent.Email, user.Language);
            }
        }
    }
}