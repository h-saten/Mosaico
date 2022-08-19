using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Org.BouncyCastle.Security;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Commands.Unsubscribe
{
    public class UnsubscribeCommandHandler : IRequestHandler<UnsubscribeCommand>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly ICurrentUserContext _currentUser;
        private readonly IBusinessDbContext _businessDb;
        private readonly ILogger _logger;

        public UnsubscribeCommandHandler(IEventFactory eventFactory, IBusinessDbContext businessDb, IEventPublisher eventPublisher, ICurrentUserContext currentUser, ILogger logger = null)
        {
            _eventFactory = eventFactory;
            _businessDb = businessDb;
            _eventPublisher = eventPublisher;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<Unit> Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.UserId))
            {
                throw new InvalidParameterException($"Either email or user is required");
            }
            
            var company = await _businessDb.Companies.Include(c => c.CompanySubscribers).FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);
            if (company == null)
            {
                throw new CompanyNotFoundException(request.CompanyId);
            }

            if (!string.IsNullOrWhiteSpace(request.UserId) && _currentUser.IsAuthenticated)
            {
                await UnsubscribeUserAsync(request, company, cancellationToken);
            }
            else if(!string.IsNullOrWhiteSpace(request.Email) && !string.IsNullOrWhiteSpace(request.Code))
            {
                await UnsubscribeEmailAsync(request, company, cancellationToken);
            }
            return Unit.Value;
        }

        private async Task UnsubscribeEmailAsync(UnsubscribeCommand request, Company company,
            CancellationToken cancellationToken)
        {
            var normalized = request.Email.ToUpperInvariant();
            var subscription = company.CompanySubscribers.FirstOrDefault(c => c.EmailNormalized == normalized);
            if (subscription != null)
            {
                if (!string.Equals(subscription.Code, request.Code, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ForbiddenException();
                }

                company.CompanySubscribers.Remove(subscription);
                await _businessDb.SaveChangesAsync(cancellationToken);
                await PublishEvents(request.Email);
            }

            throw new SubscriptionNotFoundException(request.Email);
        }

        private async Task UnsubscribeUserAsync(UnsubscribeCommand request, Company company, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId != request.UserId)
            {
                throw new ForbiddenException();
            }

            var subscription = company.CompanySubscribers.FirstOrDefault(c => c.UserId == _currentUser.UserId);
            if (subscription == null)
            {
                throw new SubscriptionNotFoundException(request.UserId);
            }

            _businessDb.CompanySubscribers.Remove(subscription);
            await _businessDb.SaveChangesAsync(cancellationToken);
            await PublishEvents(_currentUser.Email);
        }

        private async Task PublishEvents(string email)
        {
            var e = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies,
                new UnsubscribedFromCompany(email));
            await _eventPublisher.PublishAsync(e);
        }
    }
}