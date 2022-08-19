using System;
using System.Collections.Generic;
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
using Mosaico.Integration.Email.Abstraction;
using Serilog;

namespace Mosaico.Application.Identity.EventHandlers
{
    [EventInfo(nameof(SendEmailOnUserDeletionRequest),  "users:id")]
    [EventTypeFilter(typeof(UserDeleteRequestedEvent))]
    public class SendEmailOnUserDeletionRequest : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIndex<string, string> _urls;
        private readonly IEmailSender _emailSender;

        public SendEmailOnUserDeletionRequest(IEmailSender emailSender, UserManager<ApplicationUser> userManager, IIndex<string, string> urls, ILogger logger = null)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _urls = urls;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserDeleteRequestedEvent>();
            if (userEvent != null)
            {
                var user = await _userManager.FindByIdAsync(userEvent.Id.ToString());
                if (user == null)
                {
                    throw new UserNotFoundException(userEvent.Id);
                }
                
                var email = new Email
                {
                    Html = "Your account will be fully cleaned in 14 days. If you want to cancel this you can contact support@mosaico.ai via email",
                    Recipients = new List<string> { user.Email },
                    Subject = "Account deletion"
                };
                await _emailSender.SendAsync(email);
                _logger?.Verbose($"Account deletion email was sent");
            }
        }
    }
}