using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Permissions
{
    public interface ITokenPermissionFactory
    {
        Task AddUserPermissionsAsync(Guid tokenId, string userId, List<string> permissions);
        Task<List<string>> GetRolePermissionsAsync();
        Task<TokenPermissions> GetTokenPermissionsAsync(Token token, string userId = null, CancellationToken t = new());
    }
}