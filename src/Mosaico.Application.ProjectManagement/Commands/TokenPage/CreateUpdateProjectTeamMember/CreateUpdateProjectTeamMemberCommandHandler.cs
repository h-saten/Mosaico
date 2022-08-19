using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetProjectTeamMembers;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateProjectTeamMember
{
    public class CreateUpdateProjectTeamMemberCommandHandler : IRequestHandler<CreateUpdateProjectTeamMemberCommand, Guid>
    {
        private readonly IProjectDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICacheClient _cacheClient;

        public CreateUpdateProjectTeamMemberCommandHandler(IProjectDbContext dbContext, IEventFactory eventFactory,
            IEventPublisher eventPublisher, IMapper mapper, ICacheClient cacheClient, ILogger logger = null)
        {
            _context = dbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _cacheClient = cacheClient;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateUpdateProjectTeamMemberCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var page = await _context.TokenPages.FirstOrDefaultAsync(p => p.Id == request.PageId, cancellationToken);
                    if (page == null) throw new PageNotFoundException(request.PageId.ToString());
                    var teamMemberId = Guid.NewGuid();
                    if (!request.Id.HasValue)
                    {
                        var teamMember = _mapper.Map<PageTeamMember>(request);
                        _context.PageTeamMembers.Add(teamMember);
                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        _logger?.Verbose("Project team member successfully added");
                        await PublishAddedEventAsync(request.PageId, teamMember.Id);
                        teamMemberId = teamMember.Id;
                    }
                    else
                    {
                        var teamMember = await _context.PageTeamMembers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
                        if (teamMember == null)  throw new ProjectTeamMemberNotFoundException(request.Id.Value);
                        
                        _mapper.Map(request, teamMember);
                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        _logger?.Verbose("Project team member successfully updated");
                        await PublishUpdatedEventAsync(request.PageId, teamMember.Id);
                        teamMemberId = teamMember.Id;
                    }
                    await _cacheClient.CleanAsync($"{nameof(GetProjectTeamMembersQuery)}_{page.Id}", cancellationToken);
                    return teamMemberId;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishAddedEventAsync(Guid pageId, Guid teamMemberId)
        {
            var payload = new PageTeamMemberAdded(pageId, teamMemberId);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
        
        private async Task PublishUpdatedEventAsync(Guid pageId, Guid teamMemberId)
        {
            var payload = new PageTeamMemberUpdated(pageId, teamMemberId);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}