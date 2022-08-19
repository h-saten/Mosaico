using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Cache.Base.Abstractions
{
    public interface ICacheClient
    {
        Task CleanAsync(string key, CancellationToken token = new CancellationToken());
        Task CleanAsync(List<string> keys, CancellationToken token = new CancellationToken());
        Task<TResponse> GetAsync<TResponse>(string key, CancellationToken token = new CancellationToken());

        Task AddAsync<TResponse>(string key, TResponse body, int expiration = Constants.DefaultCacheExpiration,
            CancellationToken token = new CancellationToken());
        
        bool IsEnabled { get; }
    }
}