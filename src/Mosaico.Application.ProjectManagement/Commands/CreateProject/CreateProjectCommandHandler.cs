using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Base.Extensions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IProjectDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserManagementClient _managementClient;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IProjectPermissionFactory _permissionFactory;
        private readonly ITokenPageService _pageService;

        public CreateProjectCommandHandler(IProjectDbContext dbContext, IEventFactory eventFactory, IEventPublisher eventPublisher,
            IMapper mapper, IUserManagementClient managementClient, 
            ICurrentUserContext currentUserContext, IProjectPermissionFactory permissionFactory, ITokenPageService pageService, ILogger logger = null)
        {
            _context = dbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _managementClient = managementClient;
            _currentUserContext = currentUserContext;
            _permissionFactory = permissionFactory;
            _pageService = pageService;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to create a project");
                    var projectStatus = await _context.ProjectStatuses.FirstOrDefaultAsync(s => s.Key == Domain.ProjectManagement.Constants.ProjectStatuses.New, cancellationToken: cancellationToken);
                    if (projectStatus == null)
                    {
                        throw new ProjectStatusNotFoundException(Domain.ProjectManagement.Constants.ProjectStatuses.New);
                    }
                    
                    var project = _mapper.Map<Project>(request);
                    
                    project.Slug = project.Title.Trim().ToLowerInvariant().ToSlug();
                    project.SlugInvariant = project.Slug.ToUpperInvariant();
                    if (_context.Projects.Any(p => p.SlugInvariant == project.SlugInvariant))
                    {
                        throw new ProjectAlreadyExistsException(project.Title);
                    }
                    
                    project.SetStatus(projectStatus);
                    await AssignCompanyIfAllowedAsync(request, cancellationToken, project);
                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync(cancellationToken);
                    await AddProjectOwnerAsync(project, _currentUserContext.UserId, cancellationToken);
                    await CreateTokenPageAsync(project, _currentUserContext.UserId, request.ShortDescription, cancellationToken);
                    await AddDefaultPaymentMethods(project, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Project was successfully added");
                    _logger?.Verbose($"Updating user permissions");
                    await _permissionFactory.AddUserPermissionsAsync(project.Id, _currentUserContext.UserId.ToString(), await _permissionFactory.GetRolePermissionsAsync(Domain.ProjectManagement.Constants.Roles.Owner));
                    _logger?.Verbose($"Attempting to send events");
                    await PublishProjectCreatedEventAsync(project);
                    _logger?.Verbose($"Events were sent");
                    await transaction.CommitAsync(cancellationToken);
                    return project.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task AddDefaultPaymentMethods(Project project, CancellationToken cancellationToken)
        {
            var defaultPaymentKeys = Domain.ProjectManagement.Constants.PaymentMethods.ProjectDefault;
            var defaultPaymentMethods = await _context
                .PaymentMethods
                .Where(x => defaultPaymentKeys.Contains(x.Key))
                .ToListAsync(cancellationToken);
            
            if (defaultPaymentMethods.Count != defaultPaymentKeys.Count)
            {
                throw new PaymentMethodNotExistsException(Domain.ProjectManagement.Constants.PaymentMethods.MosaicoWallet);
            }
            
            project.PaymentMethods.AddRange(defaultPaymentMethods);
        }

        private async Task AssignCompanyIfAllowedAsync(CreateProjectCommand request, CancellationToken cancellationToken, Project project)
        {
            var permissions = await _managementClient.GetUserPermissionsAsync(_currentUserContext.UserId,
                request.CompanyId, cancellationToken);
            if (permissions.All(p => p.Key != Authorization.Base.Constants.Permissions.Company.CanEditDetails))
            {
                throw new ForbiddenException();
            }

            project.CompanyId = request.CompanyId;
        }

        private async Task CreateTokenPageAsync(Project project, string userId, string shortDescription, CancellationToken cancellationToken)
        {
            await _pageService.CreateTokenPageAsync(project);
            _logger?.Verbose($"Token page was successfully created. Attempting to add permissions");
            var permissions = await _permissionFactory.GetRolePermissionsAsync(Domain.ProjectManagement
                .Constants.Roles
                .Owner);
            if (project.PageId.HasValue)
            {
                var shortDescriptionEntity = new ShortDescription
                {
                    Page = project.Page,
                    PageId = project.Page.Id
                };
                shortDescriptionEntity.UpdateTranslation(shortDescription, _currentUserContext.Language);
                _context.ShortDescriptions.Add(shortDescriptionEntity);
                await _context.SaveChangesAsync(cancellationToken);
                await _permissionFactory.AddUserPermissionsAsync(project.PageId.Value, userId, permissions);
                _logger?.Verbose($"Permissions were successfully assigned to {project.CreatedById}");
            }
            else
            {
                _logger?.Warning($"Project's page id was missing. could not add permissions");
            }
        }

        private async Task AddProjectOwnerAsync(Project project, string userId, CancellationToken cancellationToken)
        {
            var ownerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Key == Domain.ProjectManagement.Constants.Roles.Owner, cancellationToken: cancellationToken);
            if (ownerRole == null)
            {
                throw new ProjectRoleNotFoundException(Domain.ProjectManagement.Constants.Roles.Owner);
            }
            
            var user = await _managementClient.GetUserAsync(userId, cancellationToken);
            if (user != null)
            {
                project.Members.Add(new ProjectMember
                {
                    Email = user.Email,
                    Project = project,
                    ExpiresAt = DateTimeOffset.UtcNow,
                    AuthorizationCode = Guid.NewGuid().ToString(),
                    Role = ownerRole,
                    RoleId = ownerRole.Id,
                    IsInvitationSent = true,
                    AcceptedAt = DateTimeOffset.UtcNow,
                    IsAccepted = true,
                    UserId = user.Id
                });
            }
        }
        
        private async Task PublishProjectCreatedEventAsync(Project project)
        {
            var eventPayload = new ProjectCreatedEvent(project.Id, project.CreatedById);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}