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
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateProjectPartner
{
    public class CreateUpdateProjectPartnerCommandHandler : IRequestHandler<CreateUpdateProjectPartnerCommand, Guid>
    {
        private readonly IProjectDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICacheClient _cacheClient;
        
        public CreateUpdateProjectPartnerCommandHandler(IProjectDbContext dbContext, IEventFactory eventFactory,
            IEventPublisher eventPublisher, IMapper mapper, ICacheClient cacheClient, ILogger logger = null)
        {
            _context = dbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _cacheClient = cacheClient;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateUpdateProjectPartnerCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var page = await _context.TokenPages.FirstOrDefaultAsync(p => p.Id == request.PageId, cancellationToken);
                    if (page == null) throw new PageNotFoundException(request.PageId.ToString());
                    var partnerId = Guid.NewGuid();
                    var projectId = Guid.NewGuid();
                    if (!request.Id.HasValue)
                    {
                        var partner = _mapper.Map<PagePartners>(request);
                        _context.PagePartners.Add(partner);
                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        _logger?.Verbose("Project partner successfully added");
                        await PublishAddedEventAsync(request.PageId, partner.Id);
                        partnerId = partner.Id;
                    }
                    else
                    {
                        var partner = await _context.PagePartners.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
                        if (partner == null) throw new ProjectPartnerNotFoundException(request.Id.Value);
                        
                        _mapper.Map(request, partner);
                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        _logger?.Verbose("Project partner successfully updated");
                        await PublishUpdatedEventAsync(request.PageId, partner.Id);
                        partnerId = partner.Id;
                    }
                    await _cacheClient.CleanAsync($"{nameof(GetProjectPartnersQuery)}_{page.Id}", cancellationToken);
                    return partnerId;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishAddedEventAsync(Guid pageId, Guid partnerId)
        {
            var payload = new PagePartnerAdded(pageId, partnerId);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
        
        private async Task PublishUpdatedEventAsync(Guid pageId, Guid partnerId)
        {
            var payload = new PagePartnerUpdated(pageId, partnerId);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}