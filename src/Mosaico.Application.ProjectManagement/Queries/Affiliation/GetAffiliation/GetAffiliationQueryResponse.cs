using System;

namespace Mosaico.Application.ProjectManagement.Queries.Affiliation.GetAffiliation
{
    public class GetAffiliationQueryResponse
    {
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
        public decimal RewardPool { get; set; }
        public decimal RewardPercentage { get; set; }
        public bool IncludeAll { get; set; }
        public bool EverybodyCanParticipate { get; set; }
        public bool PartnerShouldBeInvestor { get; set; }
    }
}