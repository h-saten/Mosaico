using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.Queries.GetCompany;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Domain.BusinessManagement;

using Serilog;
using Constants = Mosaico.Domain.BusinessManagement.Constants;

namespace Mosaico.Application.BusinessManagement.Commands.UpdateCompany
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
    {
        private readonly IBusinessDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICacheClient _cacheClient;

        public UpdateCompanyCommandHandler(IBusinessDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, IMapper mapper, ICacheClient cacheClient, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _cacheClient = cacheClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to update a company");
                    var company = await _context.Companies
                        .FirstOrDefaultAsync(p => p.Id == request.CompanyId, cancellationToken);
                    if (company == null)
                    {
                        throw new CompanyNotFoundException(request.CompanyId);
                    }
                    _logger?.Verbose($"Company was found");

                    _mapper.Map(request, company);
                    
                    _context.Companies.Update(company);
                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Company was successfully updated");
                    _logger?.Verbose($"Attempting to send events");
                    await PublishCompanyUpdatedEventAsync(company);
                    _logger?.Verbose($"Events were sent");
                    await transaction.CommitAsync(cancellationToken);
                    await _cacheClient.CleanAsync(new List<string>
                    {
                        $"{nameof(GetCompanyQuery)}_{company.Slug}",
                        $"{nameof(GetCompanyQuery)}_{company.Id}"
                    }, cancellationToken);
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishCompanyUpdatedEventAsync(Company company)
        {
            var eventPayload = new CompanyUpdatedEvent(company.Id);
            var @event = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}