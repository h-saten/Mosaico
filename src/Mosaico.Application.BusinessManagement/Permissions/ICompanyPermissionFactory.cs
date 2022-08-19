using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Application.BusinessManagement.Permissions
{
    public interface ICompanyPermissionFactory
    {
        Task<CompanyPermissions> CreateCompanyPermissionsAsync(Company company, string userId = null, CancellationToken t = new());
        Task AddUserPermissionsAsync(Guid companyId, string userId, List<string> permissions);
        Task RemovePermissionsAsync(Guid? companyId, string userId, List<string> permissions = null, CancellationToken t = new());
        Task<List<string>> GetRolePermissionsAsync(string role);
    }
}