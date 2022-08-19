using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProjectVisitor
{
    public class UpdateProjectVisitorCommandHandler: IRequestHandler<UpdateProjectVisitorCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _currentUser;
        public UpdateProjectVisitorCommandHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUser)
        {
            _projectDbContext = projectDbContext;
            _currentUser = currentUser;
        }

        public async Task<Guid> Handle(UpdateProjectVisitorCommand request, CancellationToken cancellationToken)
        {
            var projectVisitor = _projectDbContext.ProjectVisitors.Where(x => x.ProjectId == request.ProjectId && x.UserId == _currentUser.UserId).FirstOrDefault();
            if(projectVisitor == null)
            {
                var visitor = new ProjectVisitors()
                {
                    UserId = _currentUser.UserId,
                    ProjectId = request.ProjectId
                };

                _projectDbContext.ProjectVisitors.Add(visitor);

                await _projectDbContext.SaveChangesAsync();

                return visitor.Id;
            }
            else
            {
                return projectVisitor.Id;
            }
        }
    }
}
