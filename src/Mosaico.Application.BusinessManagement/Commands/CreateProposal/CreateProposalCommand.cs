using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.BusinessManagement.Queries.GetCompanyProposals;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.BusinessManagement.Commands.CreateProposal
{
    [CacheReset(nameof(GetCompanyProposalsQuery), "{{CompanyId}}")]
    public class CreateProposalCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid CompanyId { get; set; }
        public string Title { get; set; }
        public int QuorumThreshold { get; set; }
        public Guid TokenId { get; set; }
        public string Description { get; set; }
    }
}