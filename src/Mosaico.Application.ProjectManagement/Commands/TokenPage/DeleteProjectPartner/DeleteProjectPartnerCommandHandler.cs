using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Queries.GetProjectPartners;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteProjectPartner
{
    public class DeleteProjectPartnerCommandHandler : IRequestHandler<DeleteProjectPartnerCommand>
    {
        private readonly IProjectDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICacheClient _cacheClient;

        public DeleteProjectPartnerCommandHandler(IProjectDbContext dbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, IMapper mapper, ICacheClient cacheClient, ILogger logger = null)
        {
            _context = dbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _cacheClient = cacheClient;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(DeleteProjectPartnerCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var partner = await _context.PagePartners.FirstOrDefaultAsync(x => x.Id == request.Id && x.PageId == request.PageId, cancellationToken: cancellationToken);
                    if (partner == null) throw new ProjectTeamMemberNotFoundException(request.Id);
                    var partnerName = partner.Name;
                    _context.PagePartners.Remove(partner);
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    _logger?.Verbose($"Project partner removed successfully");
                    await PublishEventAsync(request.PageId, partnerName);
                    await _cacheClient.CleanAsync($"{nameof(GetProjectPartnersQuery)}_{partner.PageId}", cancellationToken);
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
        
        private async Task PublishEventAsync(Guid pageId, string partnerName)
        {
            var payload = new PagePartnerDeleted(pageId, partnerName);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}
