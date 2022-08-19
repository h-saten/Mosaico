using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IIdentityContext _identityContext;

        public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager, IEventFactory eventFactory, IEventPublisher eventPublisher, IIdentityContext identityContext, ILogger logger = null)
        {
            _userManager = userManager;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _identityContext = identityContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }
            var codeDecodedBytes = WebEncoders.Base64UrlDecode(request.Code);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
            var result = await _userManager.ConfirmEmailAsync(user, codeDecoded);
            if (!result.Succeeded)
            {
                _logger?.Verbose($"Error occured during email confirmation of user {request.UserId}");
                _logger?.Verbose(result);
                throw new EmailConfirmationFailedException($"Error occured during email confirmation of user {request.UserId}");
            }
            
            await AddAuthorizedDevice(user, request.IP, request.AgentInfo);
            await _identityContext.SaveChangesAsync(cancellationToken);
            
            try
            {
                await PublishEventsAsync(user.Id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Failed to send notification about confirmed email for user {request.UserId}: {ex.Message}");
            }

            return Unit.Value;
        }

        private async Task AddAuthorizedDevice(ApplicationUser user, string deviceIp, string agentInfo)
        {
            var authorizedDevice = new AuthorizedDevice
            {
                User = user,
                AgentInfo = agentInfo,
                IP = deviceIp
            };
            await _identityContext.AuthorizedDevices.AddAsync(authorizedDevice);
        }

        private async Task PublishEventsAsync(string id, CancellationToken t)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users,
                new UserEmailConfirmedEvent(id));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e, t);
        }
    }
}