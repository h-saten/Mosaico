using System;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetCompanyProjects
{
    // [Cache("{{CompanyId}}_{{Skip}}_{{Take}}")]
    public class GetCompanyProjectsQuery : IRequest<PaginatedResult<ProjectDTO>>
    {
        public Guid CompanyId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}