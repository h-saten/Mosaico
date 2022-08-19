using System;
using MediatR;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectVisitors
{
    public class GetProjectVisitorsQuery : IRequest<GetProjectVisitorsQueryResponse>
    {
        public Guid ProjectId { get; set; }
    }
}
