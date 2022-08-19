using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Cache.Base.Abstractions
{
    public interface ITimeSeriesRepository
    {
        Task AddAsync<T>(string setName, double key, T item);
        Task<List<T>> GetAsync<T>(string setName, double min, double max);
        Task<T> GetLastOrDefaultAsync<T>(string setName);
    }
}