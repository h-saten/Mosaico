using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Mosaico.Domain.Mongodb.Base.Models;

namespace Mosaico.Domain.Mongodb.Base.Abstractions
{
    public interface IMongoDbBaseRepository<TEntity> where TEntity : EntityBase
    {
        void Add(TEntity obj);
        Task<TEntity> GetAsync(Guid id);
        Task<List<TEntity>> GetManyAsync();
        void Update(TEntity obj);
        Task<List<TEntity>> GetManyAsync(int skip, int take);
        Task<List<TEntity>> GetManyAsync(int skip, int take, FilterDefinition<TEntity> filter, SortDefinition<TEntity> sort);
        void Remove(Guid id);
        Task<bool> SaveChangesAsync();
    }
}
