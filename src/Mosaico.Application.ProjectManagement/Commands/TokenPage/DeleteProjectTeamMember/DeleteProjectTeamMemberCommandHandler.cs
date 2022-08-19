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
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteProjectTeamMember
{
    public class DeleteProjectTeamMemberCommandHandler : IRequestHandler<DeleteProjectTeamMemberCommand>
    {
        private readonly IProjectDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICacheClient _cacheClient;

        public DeleteProjectTeamMemberCommandHandler(IProjectDbContext dbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, IMapper mapper, ICacheClient cacheClient, ILogger logger = null)
        {
            _context = dbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _cacheClient = cacheClient;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(DeleteProjectTeamMemberCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var teamMember = await _context.PageTeamMembers.FirstOrDefaultAsync(x => x.Id == request.Id && x.PageId == request.PageId, cancellationToken: cancellationToken);
                    if (teamMember == null)  throw new ProjectTeamMemberNotFoundException(request.Id);
                    
                    var teamMemberName = teamMember.Name;
                    _context.PageTeamMembers.Remove(teamMember);
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    _logger?.Verbose($"Project team member removed successfully");
                    await PublishEventAsync(request.PageId, teamMemberName);
                    await _cacheClient.CleanAsync($"{nameof(GetProjectTeamMembersQuery)}_{teamMember.PageId}", cancellationToken);
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
        
        private async Task PublishEventAsync(Guid pageId, string teamMemberName)
        {
            var payload = new PageTeamMemberDeleted(pageId, teamMemberName);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}
