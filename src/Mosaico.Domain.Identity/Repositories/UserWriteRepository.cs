using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Domain.Identity.Repositories
{
    public class UserWriteRepository : EFRepositoryBase<IIdentityContext, ApplicationUser, string>, IUserWriteRepository
    {
        public UserWriteRepository(IIdentityContext dbContext)
        {
            Context = dbContext;
            DbSet = dbContext.Users;
        }
    
        protected override DbSet<ApplicationUser> DbSet { get; }
        protected override IIdentityContext Context { get; }
    
        public override Task<ApplicationUser> GetAsync(string identifier, CancellationToken t = new())
        {
            return DbSet.FirstOrDefaultAsync(u => u.Id == identifier || u.Email == identifier, t);
        }
    
        public async Task AddUserPermissions(string id, Dictionary<string, Guid?> permissions, CancellationToken t = new())
        {
            var user = await GetAsync(id, t);
            if (user == null) throw new UserNotFoundException(id);
            foreach (var newPermission in permissions)
            {
                var permission = await Context.Permissions.FirstOrDefaultAsync(p => p.Key == newPermission.Key, t);
                if (permission == null)
                {
                    permission = new Permission
                    {
                        Key = newPermission.Key
                    };
                    Context.Permissions.Add(permission);
                }
                                 
                var baseQuery = Context.UserToPermission.AsNoTracking().Where(u => u.UserId == id);
                UserToPermission userPermission;
                if (newPermission.Value.HasValue)
                {
                    userPermission = await baseQuery.FirstOrDefaultAsync(p => p.EntityId == newPermission.Value.Value && p.PermissionId == permission.Id, t);
                }
                else
                    userPermission = await baseQuery.FirstOrDefaultAsync(p => p.PermissionId == permission.Id, t);
    
                if (userPermission == null)
                {
                    userPermission = new UserToPermission(newPermission.Value, user.Id, permission.Id);
                    userPermission.SetUser(user);
                    userPermission.SetPermission(permission);
                    Context.UserToPermission.Add(userPermission);
                }
            }
            await Context.SaveChangesAsync(t);
        }
    
        public async Task<Guid> AddUserPermission(string id, string key, Guid? entityId = null, CancellationToken t = new())
        {
            var user = await GetAsync(id, t);
            if (user == null) throw new UserNotFoundException(id);
    
            var permission = await Context.Permissions.FirstOrDefaultAsync(p => p.Key == key, t);
            if (permission == null) throw new PermissionNotFoundException(key);
    
            var baseQuery = Context.UserToPermission.AsNoTracking();
            UserToPermission userPermission;
            if (entityId.HasValue)
            {
                userPermission = await baseQuery.FirstOrDefaultAsync(p => p.EntityId == entityId.Value && p.PermissionId == permission.Id, t);
            }
            else
                userPermission = await baseQuery.FirstOrDefaultAsync(p => p.PermissionId == permission.Id, t);
    
            if (userPermission == null)
            {
                userPermission = new UserToPermission(entityId, user.Id, permission.Id);
                user.Permissions.Add(userPermission);
                await Context.SaveChangesAsync(t);
            }
            
            return userPermission.Id;
        }

        public async Task RemoveUserPermissionsAsync(string id, Dictionary<string, Guid?> permissions, CancellationToken t = new())
        {
            var user = await GetAsync(id, t);
            if (user == null) throw new UserNotFoundException(id);
            var permissionsToRemove = new List<Guid>();
            foreach (var permission in permissions)
            {
                if (permission.Value.HasValue)
                {
                    var p = user.Permissions
                        .Where(p => p.Permission.Key == permission.Key && p.EntityId == permission.Value)
                        .Select(p => p.Id);
                    permissionsToRemove.AddRange(p);
                }
                else
                {
                    var p = user.Permissions
                        .Where(p => p.Permission.Key == permission.Key && p.EntityId == null)
                        .Select(p => p.Id);
                    permissionsToRemove.AddRange(p);
                }
            }

            if (permissionsToRemove.Any())
            {
                user.Permissions.RemoveAll(utp => permissionsToRemove.Contains(utp.Id));
                await Context.SaveChangesAsync(t);
            }
        }
    }
}