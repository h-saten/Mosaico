using System;
using System.Collections.Generic;
using Mosaico.Base.Tools;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.BusinessManagement.Entities
{
    public class Proposal : EntityBase
    {
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string Title { get; set; }
        public DateTimeOffset StartsAt { get; set; }
        public DateTimeOffset EndsAt { get; set; }
        public string CreatedByAddress { get; set; }
        public string ProposalId { get; set; }
        public int QuorumThreshold { get; set; }
        public string Network { get; set; }
        public Guid TokenId { get; set; }
        public virtual List<Vote> Votes { get; set; } = new List<Vote>();
        public string Description { get; set; }
        public DateTimeOffset? DeployedAt { get; set; }

        public VotingStatus GetStatus(IDateTimeProvider provider = null)
        {
            provider ??= new DateTimeProvider();
            var now = provider.Now();
            if (now >= StartsAt && now < EndsAt)
                return VotingStatus.Active;
            if (now >= EndsAt)
                return VotingStatus.Closed;
            return VotingStatus.Pending;
        }
    }
}