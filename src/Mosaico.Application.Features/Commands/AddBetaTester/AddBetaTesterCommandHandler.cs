using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Features.Exceptions;
using Mosaico.Domain.Features.Abstractions;
using Mosaico.Domain.Features.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Features;
using Serilog;

namespace Mosaico.Application.Features.Commands.AddBetaTester
{
    public class AddBetaTesterCommandHandler : IRequestHandler<AddBetaTesterCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IFeaturesDbContext _featuresDbContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        
        public AddBetaTesterCommandHandler(IFeaturesDbContext featuresDbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _featuresDbContext = featuresDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Guid> Handle(AddBetaTesterCommand request, CancellationToken cancellationToken)
        {
            var betaTester = await _featuresDbContext.BetaTesters
                .FirstOrDefaultAsync(t => t.UserId == request.UserId, cancellationToken);
            if (betaTester != null)
            {
                throw new BetaTesterAlreadyExistsException(request.UserId);
            }

            betaTester = new BetaTester
            {
                Type = Domain.Features.Constants.BetaTesterTypes.Default,
                EnrolledAt = DateTimeOffset.UtcNow,
                IsEnabled = true,
                UserId = request.UserId
            };
            _featuresDbContext.BetaTesters.Add(betaTester);
            await _featuresDbContext.SaveChangesAsync(cancellationToken);
            await PublishEventAsync(betaTester.Id);
            return betaTester.Id;
        }

        private async Task PublishEventAsync(Guid testerId)
        {
            var payload = new BetaTesterCreatedEvent(testerId);
            var e = _eventFactory.CreateEvent(Events.Features.Constants.EventPaths.Features, payload);
            await _eventPublisher.PublishAsync(e);
        }
    }
}