using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Application.BusinessManagement.Permissions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;
using Constants = Mosaico.Domain.BusinessManagement.Constants;

namespace Mosaico.Application.BusinessManagement.Commands.CreateVerification
{
    public class CreateVerificationCommandHandler : IRequestHandler<CreateVerificationCommand, Guid>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;

        public CreateVerificationCommandHandler(IBusinessDbContext dbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, IMapper mapper, ILogger logger = null)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
        }

        public async Task<Guid> Handle(CreateVerificationCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to create a company verification");
                    var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);
                    if (company == null)
                    {
                        throw new CompanyNotFoundException(request.CompanyId);
                    }
                    var verification = _mapper.Map<Verification>(request);
                    if (request.Shareholders?.Count ==0)
                    {
                        throw new ShareholdersNotFoundException();
                    }
                    foreach (var shareholder in request.Shareholders.Select(s => _mapper.Map<Shareholder>(s)))
                    {
                        _context.Shareholders.Add(shareholder);
                        verification.Shareholders.Add(shareholder);
                    }
                    _context.Verifications.Add(verification);
                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Verification was successfully added");
                    await PublishVerificationCreatedEventAsync(verification, company.CompanyName);
                    _logger?.Verbose($"Events were sent");
                    await transaction.CommitAsync(cancellationToken);
                    return verification.Id;
                }
                catch (Exception ex)
                {
                    var err = ex;
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishVerificationCreatedEventAsync(Verification verification, string title)
        {
            var emailList = verification.Shareholders.Select(x => x.Email).ToList();
            var eventPayload = new CompanyVerificationCreatedEvent(title, verification.CompanyId, verification.Id, emailList);
            var @event = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}