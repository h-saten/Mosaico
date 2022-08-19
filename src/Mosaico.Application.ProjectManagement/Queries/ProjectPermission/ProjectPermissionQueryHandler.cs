using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.ProjectPermission
{
    public class ProjectPermissionQueryHandler : IRequestHandler<ProjectPermissionQuery, ProjectPermissions>
    {
        private readonly IProjectPermissionFactory _permissionFactory;
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;

        public ProjectPermissionQueryHandler(IProjectDbContext projectDbContext,
            IProjectPermissionFactory permissionFactory, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _permissionFactory = permissionFactory;
            _logger = logger;
        }

        public async Task<ProjectPermissions> Handle(ProjectPermissionQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.GetProjectOrThrowAsync(request.UniqueIdentifier, cancellationToken);
            var permissions = await _permissionFactory.CreateProjectPermissionsAsync(project, request.UserId, cancellationToken);
            return permissions;
        }
    }
}