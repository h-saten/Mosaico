using MediatR;
using Mosaico.Cache.Base.Attributes;
using System;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageIntroVideo
{
    // [Cache("{{PageId}}")]
    public class GetPageIntroVideoQuery: IRequest<GetPageIntroVideoQueryResponse>
    {
        public Guid PageId { get; set; }
    }
}
