using System;
using MediatR;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyProposals
{
    // [Cache("{{CompanyId}}", ExpirationInMinutes = 5)]
    public class GetCompanyProposalsQuery : IRequest<PaginatedResult<ProposalDTO>>
    {
        public Guid CompanyId { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}