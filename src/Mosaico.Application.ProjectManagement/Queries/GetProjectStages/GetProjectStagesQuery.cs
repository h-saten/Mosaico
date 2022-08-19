using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectStages
{
    // [Cache("{{Id}}")]
    public class GetProjectStagesQuery : IRequest<GetProjectsStagesQueryResponse>
    {
        public Guid Id { get; set; }
    }
}