using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectStage
{
    // [Cache("{{ProjectId}}_{{StageId}}")]
    public class GetProjectStageQuery : IRequest<GetProjectStageQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public Guid StageId { get; set; }
    }
}