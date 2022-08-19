using System;
using Mosaico.Domain.BusinessManagement.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.BusinessManagement.DTOs
{
    public class ProposalDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset StartsAt { get; set; }
        public DateTimeOffset EndsAt { get; set; }
        public string CreatedByAddress { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VotingStatus Status { get; set; }

        public string ProposalId { get; set; }
        public int QuorumThreshold { get; set; }
        public string Network { get; set; }
        public Guid TokenId { get; set; }
        public long VoteCount { get; set; }
        public decimal ForCount { get; set; }
        public decimal AgainstCount { get; set; }
        public decimal AbstainCount { get; set; }
        public decimal ForCountPercentage { get; set; }
        public decimal AgainstCountPercentage { get; set; }
        public decimal AbstainCountPercentage { get; set; }
        public bool QuorumReached { get; set; }
        public string Description { get; set; }
    }
}