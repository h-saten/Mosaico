using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.UpdateUser
{
    //TODO: write unit tests
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IIdentityContext _identityContext;
        private readonly ILogger _logger;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserCommandHandler(IIdentityContext identityContext, IEventPublisher eventPublisher, IEventFactory eventFactory, UserManager<ApplicationUser> userManager, ILogger logger = null)
        {
            _identityContext = identityContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                throw new UserNotFoundException(request.Id);
            }

            using (var transaction = _identityContext.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"User was found in database");
                    if (user.AMLStatus != AMLStatus.Unknown && user.AMLStatus != 0)
                    {
                        throw new AMLAlreadyCompletedException();
                    }

                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.Timezone = request.Timezone;
                    user.Country = request.Country;
                    user.PostalCode = request.PostalCode;
                    user.City = request.City;
                    user.Street = request.Street;
                    user.Dob = request.Dob;
                    await _identityContext.SaveChangesAsync(cancellationToken);
                    await PublishEventsAsync(user.Id);
                    await transaction.CommitAsync(cancellationToken);
                    return Unit.Value;
                }
                catch (Exception)
                {
                    _logger?.Verbose($"Rolling back transaction");
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishEventsAsync(string userId)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserUpdatedEvent(userId));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e);
        }
    }
}