using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetTokenomics
{
    // [Cache("{{ProjectId}}", ExpirationInMinutes = 5)]
    public class GetTokenomicsQuery : IRequest<GetTokenomicsQueryResponse>
    {
        public Guid ProjectId { get; set; }
    }
}