using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Features.Entities
{
    public class BetaTester : EntityBase
    {
        public string UserId { get; set; }
        public bool IsEnabled { get; set; }
        public string Type { get; set; }
        public DateTimeOffset EnrolledAt { get; set; }
        public virtual List<TestSubmission> TestSubmissions { get; set; } = new List<TestSubmission>();
    }
}