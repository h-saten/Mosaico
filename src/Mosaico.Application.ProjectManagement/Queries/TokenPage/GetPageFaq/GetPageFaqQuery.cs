using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageFaq
{
    // [Cache("{{PageId}}_{{Language}}")]
    public class GetPageFaqQuery : IRequest<GetPageFaqQueryResponse>
    {
        public Guid PageId { get; set; }
        public string Language { get; set; }
    }
}