using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.InitiatePasswordReset
{
    public class InitiatePasswordResetCommandHandler : IRequestHandler<InitiatePasswordResetCommand>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        
        public InitiatePasswordResetCommandHandler(UserManager<ApplicationUser> userManager, IEventPublisher eventPublisher, IEventFactory eventFactory, ILogger logger = null)
        {
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(InitiatePasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user != null)
            {
                _logger?.Information($"User {request.Email} was found in database. Initiating password reset.");
                try
                {
                    await PublishEventsAsync(user.Id, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger?.Error($"Error occured during sending initiate password reset event for user {request.Email}: {ex.Message}");
                }
            }
            return Unit.Value;
        }

        private async Task PublishEventsAsync(string id, CancellationToken t)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users,
                new UserInitiatedPasswordReset(id));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e, t);
        }
    }
}