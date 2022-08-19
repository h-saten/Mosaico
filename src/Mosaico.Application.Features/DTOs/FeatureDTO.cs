using System;

namespace Mosaico.Application.Features.DTOs
{
    public class FeatureDTO
    {
        public string FeatureName { get; set; }
        public string Value { get; set; }
        public Guid? EntityId { get; set; }
    }
}