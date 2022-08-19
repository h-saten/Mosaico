using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Base;
using Mosaico.Validation.Base;
using Serilog;

namespace Mosaico.Core.EntityFramework
{
    public abstract class EFRepositoryBase<TContext, TEntity> : IRepository<TEntity> where TEntity : class, IEntity<Guid> where TContext : IDbContext
    {
        protected readonly ILogger Logger;
        protected readonly IEnumerable<IValidator<TEntity>> Validators;

        protected EFRepositoryBase(ILogger logger = null, IEnumerable<IValidator<TEntity>> validators = null)
        {
            Logger = logger;
            Validators = validators;
        }

        protected abstract DbSet<TEntity> DbSet { get; }
        protected abstract TContext Context { get; }

        public virtual Task<TEntity> GetAsync(Guid id, CancellationToken token = new CancellationToken())
        {
            return SingleAsync(entity => entity.Id == id, token);
        }
        
        public virtual async Task DeleteAsync(TEntity item, CancellationToken token = new CancellationToken())
        {
            DbSet.Remove(item);
            await Context.SaveChangesAsync(token);
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken token = new CancellationToken())
        {
            var item = await GetAsync(id);
            if (item != null)
                await DeleteAsync(item);
        }

        public virtual async Task<Guid> AddAsync(TEntity item, CancellationToken token = new CancellationToken())
        {
            DbSet.Add(item);
            await Context.SaveChangesAsync(token);
            return item.Id;
        }

        public virtual async Task<List<Guid>> AddAsync(IEnumerable<TEntity> items, CancellationToken token = new CancellationToken())
        {
            DbSet.AddRange(items);
            await Context.SaveChangesAsync(token);
            return items.Select(i => i.Id).ToList();
        }

        public virtual async Task UpdateAsync(TEntity item, CancellationToken token = new CancellationToken())
        {
            DbSet.Update(item);
            await Context.SaveChangesAsync(token);
        }

        public virtual async Task UpdateAsync(IEnumerable<TEntity> items, CancellationToken token = new CancellationToken())
        {
            DbSet.UpdateRange(items);
            await Context.SaveChangesAsync(token);
        }

        protected Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = new CancellationToken())
        {
            return DbSet.FirstOrDefaultAsync(expression, cancellationToken: token);
        }
    }
    
    public abstract class EFRepositoryBase<TContext, TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, IEntity<TId> where TContext : IDbContext where TId : class
    {
        protected readonly ILogger Logger;
        protected readonly IEnumerable<IValidator<TEntity, TId>> Validators;

        protected EFRepositoryBase(ILogger logger = null, IEnumerable<IValidator<TEntity, TId>> validators = null)
        {
            Logger = logger;
            Validators = validators;
        }

        protected abstract DbSet<TEntity> DbSet { get; }
        protected abstract TContext Context { get; }

        public virtual Task<TEntity> GetAsync(TId id, CancellationToken token = new CancellationToken())
        {
            return SingleAsync(entity => entity.Id == id, token);
        }
        
        public virtual async Task DeleteAsync(TEntity item, CancellationToken token = new CancellationToken())
        {
            DbSet.Remove(item);
            await Context.SaveChangesAsync(token);
        }

        public virtual async Task DeleteAsync(TId id, CancellationToken token = new CancellationToken())
        {
            var item = await GetAsync(id, token);
            if (item != null)
                await DeleteAsync(item, token);
        }

        public virtual async Task<TId> AddAsync(TEntity item, CancellationToken token = new CancellationToken())
        {
            DbSet.Add(item);
            await Context.SaveChangesAsync(token);
            return item.Id;
        }

        public virtual async Task<List<TId>> AddAsync(IEnumerable<TEntity> items, CancellationToken token = new CancellationToken())
        {
            DbSet.AddRange(items);
            await Context.SaveChangesAsync(token);
            return items.Select(i => i.Id).ToList();
        }

        public virtual async Task UpdateAsync(TEntity item, CancellationToken token = new CancellationToken())
        {
            DbSet.Update(item);
            await Context.SaveChangesAsync(token);
        }

        public virtual async Task UpdateAsync(IEnumerable<TEntity> items, CancellationToken token = new CancellationToken())
        {
            DbSet.UpdateRange(items);
            await Context.SaveChangesAsync(token);
        }

        protected Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = new CancellationToken())
        {
            return DbSet.FirstOrDefaultAsync(expression, cancellationToken: token);
        }
    }
}