using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Permissions
{
    public interface IProjectPermissionFactory
    {
        Task<ProjectPermissions> CreateProjectPermissionsAsync(Project project, string userId = null, CancellationToken t = new());
        Task<bool> GetUserAbilityToPurchaseAsync(Project project, string userId, CancellationToken t = new());
        Task<bool> GetUserAbilityToPurchaseAsync(Guid projectId, string userId, CancellationToken t = new());
        Task AddUserPermissionsAsync(Guid projectId, string userId, List<string> permissions);
        Task RemovePermissionsAsync(Guid? projectId, string userId, List<string> permissions = null, CancellationToken t = new());
        Task<List<string>> GetRolePermissionsAsync(string role);
    }
}