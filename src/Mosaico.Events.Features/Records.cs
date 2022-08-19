using System;

namespace Mosaico.Events.Features
{
    public record FeatureCreatedEvent(Guid Id);
    public record FeatureUpdatedEvent(Guid Id);

    public record BetaTesterCreatedEvent(Guid Id);

}