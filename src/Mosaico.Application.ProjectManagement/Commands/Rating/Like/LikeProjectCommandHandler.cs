using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Ratings;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.Rating.Like
{
    public class LikeProjectCommandHandler : IRequestHandler<LikeProjectCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _currentUserContext;

        public LikeProjectCommandHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUserContext)
        {
            _projectDbContext = projectDbContext;
            _currentUserContext = currentUserContext;
        }

        public async Task<Unit> Handle(LikeProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(t => t.Id == request.ProjectId, cancellationToken);
            if (project == null) throw new ProjectNotFoundException(request.ProjectId);
            if(!project.Likes.Any(l => l.UserId == _currentUserContext.UserId))
            {
                _projectDbContext.ProjectLikes.Add(new ProjectLike
                {
                    Project = project,
                    ProjectId = project.Id,
                    UserId = _currentUserContext.UserId
                });
                await _projectDbContext.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}