using System;

namespace Mosaico.SDK.Features.Models
{
    public class MosaicoBetaTester
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public bool IsEnabled { get; set; }
        public string Type { get; set; }
        public DateTimeOffset EnrolledAt { get; set; }
    }
}