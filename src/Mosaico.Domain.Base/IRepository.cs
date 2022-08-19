using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Base;

namespace Mosaico.Domain.Base
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Guid id, CancellationToken token = new CancellationToken());
        Task DeleteAsync(T item, CancellationToken token = new CancellationToken());
        Task DeleteAsync(Guid id, CancellationToken token = new CancellationToken());
        Task<Guid> AddAsync(T item, CancellationToken token = new CancellationToken());
        Task<List<Guid>> AddAsync(IEnumerable<T> items, CancellationToken token = new CancellationToken());
        Task UpdateAsync(T item, CancellationToken token = new CancellationToken());
        Task UpdateAsync(IEnumerable<T> items, CancellationToken token = new CancellationToken());
    }
    
    public interface IRepository<T, TId> where T : class
    {
        Task<T> GetAsync(TId id, CancellationToken token = new CancellationToken());
        Task DeleteAsync(T item, CancellationToken token = new CancellationToken());
        Task DeleteAsync(TId id, CancellationToken token = new CancellationToken());
        Task<TId> AddAsync(T item, CancellationToken token = new CancellationToken());
        Task<List<TId>> AddAsync(IEnumerable<T> items, CancellationToken token = new CancellationToken());
        Task UpdateAsync(T item, CancellationToken token = new CancellationToken());
        Task UpdateAsync(IEnumerable<T> items, CancellationToken token = new CancellationToken());
    }
}