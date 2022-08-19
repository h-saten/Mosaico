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
    [EventInfo(nameof(SendChangePasswordEmailInitiated),  "users:id")]
    [EventTypeFilter(typeof(UserInitiatedPasswordChange))]
    public class SendChangePasswordEmailInitiated : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IIdentityEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SendChangePasswordEmailInitiated(IIdentityEmailService emailService, UserManager<ApplicationUser> userManager, ILogger logger = null)
        {
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserInitiatedPasswordChange>();
            if (userEvent != null)
            {
                var user = await _userManager.FindByIdAsync(userEvent.Id.ToString());
                if (user == null)
                {
                    throw new UserNotFoundException(userEvent.Id);
                }
                
                await _emailService.SendPasswordChangeConfirmationCodeEmailAsync(user.Email, userEvent.code, user.Language);
            }
        }
    }
}