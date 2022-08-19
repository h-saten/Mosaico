using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Mosaico.Domain.Mongodb.Base.Abstractions
{
    public interface IMongoDBContext
    {
        void AddCommand(Func<Task> func);
        Task<int> SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
