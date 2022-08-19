using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Base;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.Abstractions
{
    public interface IUserReadRepository
    {
        Task<ApplicationUser> GetAsync(string identifier, CancellationToken t = new());

        Task<PaginatedResult<ApplicationUser>> GetUsersAsync(string userName = null,
            CancellationToken token = new CancellationToken());
        Task<PaginatedResult<ApplicationUser>> GetAsync(string firstName = null, string email = null, int skip = 0, int take = 10, CancellationToken token = new());
        Task<List<UserToPermission>> GetUserPermissionsAsync(string identifier, CancellationToken t = new());
        Task<UserToPermission> GetUserPermission(string identifier, string key, CancellationToken t = new());
        Task<List<UserToPermission>> GetUserPermissionsAsync(string identifier, Guid entityId, CancellationToken t = new());
        Task<PaginatedResult<ApplicationUser>> GetUsersWithPermission(string key, CancellationToken t = new());
        Task<PaginatedResult<DeletionRequest>> GetDeletionRequestsAsync(int skip = 0, int take = 30, CancellationToken token = new CancellationToken());
        Task<List<ApplicationUser>> GetUsersAsync(List<string> usersId, CancellationToken token = new());

    }
}