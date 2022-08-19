using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.ProjectManagement.Commands.Administration.ApproveProject
{
    public class ApproveProjectCommandHandler : IRequestHandler<ApproveProjectCommand>
    {
        private readonly IProjectDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IValidator<Project> _projectValidator;
        private readonly ILogger _logger;

        public ApproveProjectCommandHandler(IProjectDbContext dbContext, IEventPublisher eventPublisher, IEventFactory eventFactory, ILogger logger, IValidator<Project> projectValidator)
        {
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
            _projectValidator = projectValidator;
        }

        public async Task<Unit> Handle(ApproveProjectCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to approve project");
            var project = await _dbContext.GetProjectOrThrowAsync(request.ProjectId, cancellationToken);
            _logger?.Verbose($"Project was found");
                    
            var newStatus = await _dbContext.ProjectStatuses.FirstOrDefaultAsync(p => p.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Approved, cancellationToken: cancellationToken);
            if (newStatus == null)
            {
                throw new ProjectStatusNotFoundException(Domain.ProjectManagement.Constants.ProjectStatuses.Approved);
            }

            if (project.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.UnderReview)
            {
                throw new ProjectStatusChangeException(newStatus.Key, project.Id.ToString());
            }
            
            project.SetStatus(newStatus);
            
            var results = await _projectValidator.ValidateAsync(project, strategy => strategy.IncludeAllRuleSets(), cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(Domain.ProjectManagement.Constants.ErrorCodes.InvalidProject, 
                    results.Errors.Select(e => e.ErrorCode));
            }
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger?.Verbose($"Saving project and sending events");
            await PublishProjectAcceptedEventAsync(project);
            return Unit.Value;
        }
        
        private async Task PublishProjectAcceptedEventAsync(Project project)
        {
            var eventPayload = new ProjectAcceptedEvent(project.Id);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}