using System;

namespace Mosaico.Application.ProjectManagement.DTOs.Affiliation
{
    public class UserAffiliationPartnerDTO
    {
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectSlug { get; set; }
        public int TransactionsCount { get; set; }
        public decimal EstimatedReward { get; set; }
        public TokenDTO Token { get; set; }
    }
}