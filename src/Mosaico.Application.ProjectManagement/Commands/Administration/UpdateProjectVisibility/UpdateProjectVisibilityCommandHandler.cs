using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.Administration.UpdateProjectVisibility
{
    public class UpdateProjectVisibilityCommandHandler : IRequestHandler<UpdateProjectVisibilityCommand>
    {
        private readonly IProjectDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly ILogger _logger;
        
        public UpdateProjectVisibilityCommandHandler(IProjectDbContext dbContext, IEventPublisher eventPublisher, IEventFactory eventFactory, ILogger logger = null)
        {
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateProjectVisibilityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger?.Verbose($"Attempting to update project visibility");
                var project = await _dbContext.GetProjectOrThrowAsync(request.Id, cancellationToken);
                _logger?.Verbose($"Project was found");

                project.IsVisible=request.Visibility;

                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"Saving project visibility and sending events");

                await PublishProjectVisibilityEventAsync(project.Id);

                return Unit.Value;
            }
            catch (Exception)
            {
                _logger?.Verbose($"Something went wrong during visibility update");
                throw;
            }
        }
        
        private async Task PublishProjectVisibilityEventAsync(Guid Id)
        {
            var eventPayload = new ProjectVisibilityEvent(Id);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}