using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Domain.Identity.Repositories
{
    //TODO: to Redis
    public class UserReadRepository : IUserReadRepository
    {
        private readonly IIdentityContext _dbContext;
    
        public UserReadRepository(IIdentityContext dbContext)
        {
            _dbContext = dbContext;
        }
    
        public async Task<ApplicationUser> GetAsync(string identifier, CancellationToken t = new())
        {
            var query = _dbContext.Users
                .Include(u => u.Permissions).ThenInclude(up => up.Permission)
                .AsNoTracking();
            var user = await query.FirstOrDefaultAsync(u => u.Id == identifier || u.Email == identifier, t);
            return user;
        }
        public virtual async Task<PaginatedResult<ApplicationUser>> GetAsync(string firstName = null, string email = null, int skip = 0, int take = 10, CancellationToken token = new())
        {
            var itemsQuery = _dbContext
                .Users
                .Where(u=> 
                       (firstName == null || u.FirstName.Contains(firstName))
                    && (email == null || u.Email.Contains(email)));
            
            var items =  await itemsQuery.Skip(skip)
                .Take(take)
                .ToListAsync(token);
            var totalItems = await itemsQuery.CountAsync(token);
            return new PaginatedResult<ApplicationUser>
            {
                Entities = items,
                Total = totalItems
            };
        }

        public virtual async Task<PaginatedResult<ApplicationUser>> GetUsersAsync(string userName = null, CancellationToken token = new())
        {
            var users = await _dbContext
                .Users
                .Where(a => userName == null || a.UserName.Contains(userName))
                .ToListAsync(token);
            
            var totalUsers = users.Count;
            return new PaginatedResult<ApplicationUser>
            {
                Entities = users,
                Total = totalUsers
            };
        }

        public virtual async Task<PaginatedResult<DeletionRequest>> GetDeletionRequestsAsync(int skip = 0, int take = 10, CancellationToken token = new CancellationToken())
        {

            var items = await _dbContext.DeletionRequests.Include(x=>x.User).Skip(skip).Take(take).ToListAsync(cancellationToken: token);
            var totalItems = await _dbContext.DeletionRequests.CountAsync(cancellationToken: token);
            return new PaginatedResult<DeletionRequest>
            {
                Entities = items,
                Total = totalItems
            };
        }

        public virtual async Task<PaginatedResult<ApplicationUser>> GetUsersWithPermission(string key, CancellationToken t = new())
        {
            var permissions = await _dbContext.UserToPermission.Where(x => x.Permission.Key == key).AsNoTracking().ToListAsync(cancellationToken: t);
            var userIdList = permissions.Select(x => x.UserId).ToList();
            var items = _dbContext.Users.Where(x=>userIdList.Contains(x.Id)).AsNoTracking();
            var result = await items.ToListAsync(cancellationToken: t);
            var totalItems = await items.CountAsync(cancellationToken: t);
            return new PaginatedResult<ApplicationUser>
            {
                Entities = result,
                Total = totalItems
            };
        }
        public async Task<UserToPermission> GetUserPermission(string identifier, string key, CancellationToken t = new())
        {
            var user = await GetAsync(identifier, t);
    
            if (user == null) 
                throw new UserNotFoundException(identifier);
    
            return user.Permissions.FirstOrDefault(p => p.Permission.Key == key);
        }
    
        public async Task<List<UserToPermission>> GetUserPermissionsAsync(string identifier, CancellationToken t = new())
        {
            var user = await GetAsync(identifier, t);
    
            if (user == null) 
                throw new UserNotFoundException(identifier);
    
            return user.Permissions;
        }
        
        public async Task<List<UserToPermission>> GetUserPermissionsAsync(string identifier, Guid entityId, CancellationToken t = new())
        {
            var user = await GetAsync(identifier, t);
    
            if (user == null) 
                throw new UserNotFoundException(identifier);
    
            return user.Permissions.Where(p => p.EntityId == entityId).ToList();
        }
        
        public Task<List<ApplicationUser>> GetUsersAsync(List<string> usersId, CancellationToken token = new CancellationToken())
        {
            return _dbContext
                .Users
                .AsNoTracking()
                .Where(u => usersId.Contains(u.Id))
                .ToListAsync(token);
        }
    }
}