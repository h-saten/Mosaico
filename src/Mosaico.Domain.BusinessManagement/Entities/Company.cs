using System;
using System.Collections.Generic;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.BusinessManagement.Entities
{
    public class Company : EntityBase
    {
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        
        [Encrypted]
        public string VATId { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public bool IsApproved { get; set; }
        public DateTimeOffset? ApprovedAt { get; set; }
        public string ApprovedById { get; set; }
        public string LegacyId { get; set; }
        public virtual List<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public string Size { get; set; }
        public string LogoUrl { get; set; }
        public string Network { get; set; }
        public string ContractAddress { get; set; }
        public bool IsVotingEnabled { get; set; }
        public bool OnlyOwnerProposals { get; set; }
        public VotingPeriod InitialVotingDelay { get; set; }
        public VotingPeriod InitialVotingPeriod { get; set; }
        public long Quorum { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public virtual List<CompanySubscriber> CompanySubscribers { get; set; } = new List<CompanySubscriber>();
        public string Slug { get; set; }
        public virtual List<Proposal> Proposals { get; set; } = new List<Proposal>();
        public virtual List<Document> Documents { get; set; } = new List<Document>();
    }
}