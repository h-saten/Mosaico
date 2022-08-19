using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectPartners
{
    // [Cache("{{PageId}}")]
    public class GetProjectPartnersQuery : IRequest<GetProjectPartnersQueryResponse>
    {
        public Guid PageId { get; set; }
    }
}