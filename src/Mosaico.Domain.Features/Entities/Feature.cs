using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Features.Entities
{
    public class Feature : EntityBase
    {
        public string FeatureName { get; set; }
        public string Value { get; set; }
        public Guid? EntityId { get; set; }
        public bool IsGloballyAvailable { get; set; }
        public string Category { get; set; }
    }
}