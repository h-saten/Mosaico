using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetArticles
{
    public class GetArticlesQuery : IRequest<GetArticlesQueryResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public Guid ProjectId { get; set; }

    }
}