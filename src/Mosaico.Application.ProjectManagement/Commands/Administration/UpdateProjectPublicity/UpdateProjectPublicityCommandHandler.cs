using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.Administration.UpdateProjectPublicity
{
    public class UpdateProjectPublicityCommandHandler : IRequestHandler<UpdateProjectPublicityCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICacheClient _cacheClient;

        public UpdateProjectPublicityCommandHandler(IProjectDbContext projectDbContext, ICacheClient cacheClient)
        {
            _projectDbContext = projectDbContext;
            _cacheClient = cacheClient;
        }

        public async Task<Unit> Handle(UpdateProjectPublicityCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            project.IsAccessibleViaLink = request.IsPublic;
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            await _cacheClient.CleanAsync(new List<string>
            {
                $"{nameof(GetProjectQuery)}_{project.Id}",
                $"{nameof(GetProjectQuery)}_{project.Slug}"
            }, cancellationToken);
            return Unit.Value;
        }
    }
}