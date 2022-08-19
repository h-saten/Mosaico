using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Features.Abstractions;
using Mosaico.Domain.Features.Entities;
using Mosaico.Domain.Features.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Features;
using Serilog;
using Mosaico.Domain.Features;
using Constants = Mosaico.Domain.Features.Constants;

namespace Mosaico.Application.Features.Commands.UpsertSetting
{
    public class UpsertSettingCommandHandler : IRequestHandler<UpsertSettingCommand>
    {
        private readonly ILogger _logger;
        private readonly IFeaturesDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;

        public UpsertSettingCommandHandler(ILogger logger, IFeaturesDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher)
        {
            _logger = logger;
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
        }

        public async Task<Unit> Handle(UpsertSettingCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose("Attempting to create setting");
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var feature = await _context.Features
                        .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                    var isNewFeature = feature is null;

                    if (isNewFeature)
                    {
                        feature = new Feature
                        {
                            Value = request.Value,
                            EntityId = request.EntityId,
                            FeatureName = request.FeatureName
                        };
                        _context.Features.Add(feature);
                    }
                    else
                    {
                        feature.Value = request.Value;
                        feature.EntityId = request.EntityId;
                        feature.FeatureName = request.FeatureName;
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    if (isNewFeature)
                    {
                        await PublishFeatureCreatedEventAsync(feature);
                    } else
                    {
                        await PublishFeatureUpdatedEventAsync(feature);
                    }
                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    _logger?.Error($"{ex.Message} - {ex.StackTrace}");
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishFeatureCreatedEventAsync(Feature feature)
        {
            var eventPayload = new FeatureCreatedEvent(feature.Id);
            var @event = _eventFactory.CreateEvent(Events.Features.Constants.EventPaths.Features, eventPayload);
            await _eventPublisher.PublishAsync(@event);
        }

        private async Task PublishFeatureUpdatedEventAsync(Feature feature)
        {
            var eventPayload = new FeatureUpdatedEvent(feature.Id);
            var @event = _eventFactory.CreateEvent(Events.Features.Constants.EventPaths.Features, eventPayload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}