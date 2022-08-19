using System;
using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs.Affiliation;

namespace Mosaico.Application.ProjectManagement.Queries.Affiliation.GetUserAffiliation
{
    public class GetUserAffiliationQueryResponse
    {
        public Guid Id { get; set; }
        public string AccessCode { get; set; }
        public List<UserAffiliationPartnerDTO> Projects { get; set; } = new List<UserAffiliationPartnerDTO>();
    }
}