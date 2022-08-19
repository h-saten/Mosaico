using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Mosaico.Cache.Base
{
    public interface ICacheRepository<T> where T : CacheItemBase
    {
        Task CreateAsync(T item, TimeSpan? expiryTime = null);
        Task CreateAsync(IEnumerable<T> items);
        Task<T> GetAsync(string name);
        Task<ReadOnlyCollection<T>> GetAsync(IEnumerable<string> names);
        Task<ReadOnlyCollection<T>> GetAsync(int take = 10, int skip = 0);
        Task DeleteAsync(string name);
        Task DeleteAsync(T item);
        Task DeleteAsync(IEnumerable<string> ids);
        Task DeleteAsync(IEnumerable<T> items);
    }
}