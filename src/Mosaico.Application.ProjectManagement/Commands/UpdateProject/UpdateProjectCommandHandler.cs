using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;
using Mosaico.SDK.Wallet.Models;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
    {
        private readonly IProjectDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IWalletClient _walletClient;
        private readonly ICacheClient _cacheClient;
        private readonly ITokenPageDispatcher _pageDispatcher;

        public UpdateProjectCommandHandler(IProjectDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, IMapper mapper, IWalletClient walletClient, ICacheClient cacheClient, ITokenPageDispatcher pageDispatcher, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _walletClient = walletClient;
            _cacheClient = cacheClient;
            _pageDispatcher = pageDispatcher;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var logoChanged = false;
                    _logger?.Verbose($"Attempting to update a project");
                    var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                    if (project == null)
                    {
                        throw new ProjectNotFoundException(request.ProjectId);
                    }
                    _logger?.Verbose($"Project was found");
                    var token = await GetTokenAsync(request, project);
                    _mapper.Map(request.Project, project);
                    if (token != null)
                    {
                        if (project.LogoUrl != token.LogoUrl)
                        {
                            project.LogoUrl = token.LogoUrl;
                            logoChanged = true;
                        }
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Project was successfully updated");
                    _logger?.Verbose($"Attempting to send events");
                    await PublishProjectUpdatedEventAsync(project);
                    _logger?.Verbose($"Events were sent");
                    await transaction.CommitAsync(cancellationToken);
                    await _cacheClient.CleanAsync(new List<string>
                    {
                        $"{nameof(GetProjectQuery)}_{project.Slug}",
                        $"{nameof(GetProjectQuery)}_{project.Id}"
                    }, cancellationToken);
                    if (logoChanged)
                    {
                        await _pageDispatcher.DispatchLogoUpdatedAsync(project.LogoUrl);
                    }
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task<MosaicoToken> GetTokenAsync(UpdateProjectCommand request, Project project)
        {
            if (request.Project.TokenId.HasValue && project.TokenId != request.Project.TokenId && request.Project.TokenId != Guid.Empty)
            {
                var token = await _walletClient.GetTokenAsync(request.Project.TokenId.Value);
                if (token == null)
                {
                    throw new TokenNotFoundException(request.Project.TokenId.Value);
                }
                return token;
            }

            return null;
        }

        private async Task PublishProjectUpdatedEventAsync(Project project)
        {
            var eventPayload = new ProjectUpdatedEvent(project.Id);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}