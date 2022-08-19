using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Mosaico.Domain.Mongodb.Base.Abstractions;
using Mosaico.Domain.Mongodb.Base.Models;

namespace Mosaico.Domain.Mongodb.Base.Repository
{
    public abstract class MongoDbBaseRepository<TEntity> : IMongoDbBaseRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly IMongoDBContext MongoContext;
        protected readonly IMongoCollection<TEntity> DbCollection;

        protected MongoDbBaseRepository(IMongoDBContext context)
        {
            MongoContext = context;
            DbCollection = MongoContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Add(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }
            MongoContext.AddCommand(() => DbCollection.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            var data = await DbCollection.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<List<TEntity>> GetManyAsync()
        {
            var all = await DbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual Task<List<TEntity>> GetManyAsync(int skip, int take)
        {
            return DbCollection.Find(Builders<TEntity>.Filter.Empty)
                .SortBy(t => t.CreatedAt)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }
        
        public virtual Task<List<TEntity>> GetManyAsync(int skip, int take, FilterDefinition<TEntity> filter, SortDefinition<TEntity> sort)
        {
            return DbCollection.Find(filter)
                .Sort(sort)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }

        public virtual void Update(TEntity obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            }
            var t = obj.GetType();
            MongoContext.AddCommand(() => DbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", (ObjectId)t.GetField("_id").GetValue(obj)), obj));
        }

        public virtual void Remove(Guid id)
        {
            MongoContext.AddCommand(() => DbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            var saveData = await MongoContext.SaveChanges();
            return saveData > 0;
        }
    }
}
