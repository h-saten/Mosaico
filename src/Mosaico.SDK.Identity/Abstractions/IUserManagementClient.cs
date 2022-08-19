using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.SDK.Identity.Models;

namespace Mosaico.SDK.Identity.Abstractions
{
    public interface IUserManagementClient
    {
        Task<MosaicoUser> GetCurrentUserAsync(CancellationToken cToken = new());
        Task<MosaicoUser> GetUserAsync(string identifier, CancellationToken token = new());
        Task<List<MosaicoUser>> GetUsersAsync(List<string> identifiers, CancellationToken token = new());
        Task<List<MosaicoUser>> GetUsersByNameAsync(string userName, CancellationToken token = new CancellationToken());
        Task<List<MosaicoPermission>> GetUserPermissionsAsync(string identifier, CancellationToken cToken = new CancellationToken());
        Task<List<MosaicoPermission>> GetUserPermissionsAsync(string identifier, Guid entityId, CancellationToken cToken = new CancellationToken());
        Task<List<MosaicoUser>> GetUsersWithPermission(string key, CancellationToken t = new());
        Task<bool> AccountExist(string email);
        Task<bool> RegisterAccountAsync(string email, string password, string language, bool joinNewsletter = false);
        Task<bool> RegisterConfirmedAccountAsync(string email, string password, string language, bool joinNewsletter = false);
        Task<bool> CreateKangaUserIfNotExist(string identifier, CancellationToken t = new());
        Task<MosaicoUser> GetUserByEmailAsync(string email, CancellationToken t = new());
        Task<KangaUserAccount> GetUserKangaAccountAsync(CancellationToken cToken = new());
        Task<string> CreateExternalAccountAsync(string email);
    }
}