using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Features.Entities
{
    public class TestSubmission : EntityBase
    {
        public Guid BetaTesterId { get; set; }
        public virtual BetaTester BetaTester { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsAccepted { get; set; }
        public DateTimeOffset? AcceptedAt { get; set; }
        public int Reward { get; set; }
        public bool IsRewarded { get; set; }
        public DateTimeOffset? RewardedAt { get; set; }
    }
}