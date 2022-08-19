using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectVisitors
{
    public class GetProjectVisitorsQueryHandler: IRequestHandler<GetProjectVisitorsQuery, GetProjectVisitorsQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _currentUser;
        public GetProjectVisitorsQueryHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUser)
        {
            _projectDbContext = projectDbContext;
            _currentUser = currentUser;
        }

        public async Task<GetProjectVisitorsQueryResponse> Handle(GetProjectVisitorsQuery request, CancellationToken cancellationToken)
        {
            var projectVisitor = await _projectDbContext.ProjectVisitors.AsNoTracking().FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId && x.UserId == _currentUser.UserId, cancellationToken);
            if(projectVisitor == null)
            {
                return new GetProjectVisitorsQueryResponse { isProjectVisited = false };
            }
            else
            {
                return new GetProjectVisitorsQueryResponse { isProjectVisited = true };
            }
        }
    }
}
