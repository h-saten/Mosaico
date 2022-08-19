using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.BusinessManagement.Queries.GetCompanyProposals;
using Mosaico.Cache.Base.Attributes;
using Mosaico.Domain.BusinessManagement.Entities;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.BusinessManagement.Commands.Vote
{
    [CacheReset(nameof(GetCompanyProposalsQuery), "{{CompanyId}}")]
    public class VoteCommand : IRequest
    {
        [JsonIgnore]
        public Guid CompanyId { get; set; }
        [JsonIgnore]
        public Guid ProposalId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VoteResult Result { get; set; }
    }
}