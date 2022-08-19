using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.VerifyUser
{
    public class InitiateUserVerificationCommandHandler : IRequestHandler<InitiateUserVerificationCommand>
    {
        private readonly IUserWriteRepository _repository;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
    
        public InitiateUserVerificationCommandHandler(IUserWriteRepository repository, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _repository = repository;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }
    
        public async Task<Unit> Handle(InitiateUserVerificationCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetAsync(request.UserId, cancellationToken);
    
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }
    
            user.AMLStatus = AMLStatus.Pending;
            user.IsAMLVerificationDisabled = true;
            
            await _repository.UpdateAsync(user, cancellationToken);
            await PublishUserCreatedEventAsync(user);
    
            return Unit.Value;
        }
        
        private async Task PublishUserCreatedEventAsync(ApplicationUser user)
        {
            var eventPayload = new UserVerifiedEvent(user.Id);
            var @event = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}