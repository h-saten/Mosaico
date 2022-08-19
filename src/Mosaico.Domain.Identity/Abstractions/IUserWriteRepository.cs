using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Domain.Base;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.Abstractions
{
    public interface IUserWriteRepository : IRepository<ApplicationUser, string>
    {
        Task<Guid> AddUserPermission(string id, string key, Guid? entityId = null, CancellationToken t = new CancellationToken());
        Task AddUserPermissions(string id, Dictionary<string, Guid?> permissions, CancellationToken t = new());
        Task RemoveUserPermissionsAsync(string id, Dictionary<string, Guid?> permissions, CancellationToken t = new());
    }
}