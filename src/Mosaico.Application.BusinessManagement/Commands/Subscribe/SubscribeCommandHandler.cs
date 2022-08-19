using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Base.Abstractions;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Org.BouncyCastle.Security;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Commands.Subscribe
{
    public class SubscribeCommandHandler: IRequestHandler<SubscribeCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IBusinessDbContext _businessDb;
        private readonly ICurrentUserContext _currentUser;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IStringHasher _stringHasher;
        
        public SubscribeCommandHandler(IBusinessDbContext businessDb, ICurrentUserContext currentUser, IEventPublisher eventPublisher, IEventFactory eventFactory, IStringHasher stringHasher, ILogger logger = null)
        {
            _businessDb = businessDb;
            _currentUser = currentUser;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _stringHasher = stringHasher;
            _logger = logger;
        }

        public async Task<Guid> Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            var company = await _businessDb.Companies.Include(c => c.CompanySubscribers).FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);
            if (company == null)
            {
                throw new CompanyNotFoundException(request.CompanyId);
            }

            if (!string.IsNullOrWhiteSpace(request.UserId) && _currentUser.IsAuthenticated)
            {
                return await SubscribeUserAsync(request, company);
            }
            else
            {
                return await SubscribeEmailAsync(request, company);
            }
        }

        private async Task<Guid> SubscribeUserAsync(SubscribeCommand request, Company company)
        {
            if (!string.Equals(request.UserId, _currentUser.UserId, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ForbiddenException();
            }

            var anySubscriber = company.CompanySubscribers.Any(c => c.UserId == _currentUser.UserId);
            if (anySubscriber)
            {
                throw new SubscriptionAlreadyExistsException(request.UserId);
            }

            var emailNormalized = _currentUser.Email.ToUpperInvariant();
            var subscription = company.CompanySubscribers.FirstOrDefault(c => c.EmailNormalized == emailNormalized);
            if (subscription != null)
            {
                company.CompanySubscribers.Remove(subscription);
            }

            subscription = new CompanySubscriber
            {
                Company = company,
                CompanyId = company.Id,
                Email = _currentUser.Email,
                UserId = _currentUser.UserId,
                EmailNormalized = emailNormalized,
                Code = _stringHasher.CreateHash(Guid.NewGuid().ToString())
            };

            await AddSubscriptionAsync(subscription);
            return subscription.Id;
        }

        private async Task<Guid> SubscribeEmailAsync(SubscribeCommand request, Company company)
        {
            var normalized = request.Email.ToUpperInvariant();
            var subscription = company.CompanySubscribers.FirstOrDefault(c => c.EmailNormalized == normalized);
            if (subscription == null)
            {
                throw new SubscriptionAlreadyExistsException(request.Email);
            }

            subscription = new CompanySubscriber
            {
                CompanyId = company.Id,
                Company = company,
                Email = request.Email,
                EmailNormalized = normalized,
                Code = _stringHasher.CreateHash(Guid.NewGuid().ToString())
            };
            await AddSubscriptionAsync(subscription);
            return subscription.Id;
        }

        private async Task AddSubscriptionAsync(CompanySubscriber subscriber)
        {
            _businessDb.CompanySubscribers.Add(subscriber);
            await _businessDb.SaveChangesAsync();
            await PublishEventsAsync(subscriber.Id);
        }

        private async Task PublishEventsAsync(Guid subscriptionId)
        {
            var e = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies,
                new SubscribedToCompany(subscriptionId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}