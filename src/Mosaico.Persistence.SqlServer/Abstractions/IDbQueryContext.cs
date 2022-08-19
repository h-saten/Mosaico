using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Persistence.SqlServer.Abstractions
{
    public interface IDbQueryContext
    {
        Task<IEnumerable<T>> SelectMany<T>(string query, object queryParams = null);
        Task<T> Single<T>(string query, object queryParams = null);
        Task<T> FirstOrDefault<T>(string query, object queryParams = null);
    }
}