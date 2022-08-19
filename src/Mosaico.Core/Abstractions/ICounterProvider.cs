using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Core.Abstractions
{
    public interface ICounterProvider
    {
        Task<KeyValuePair<string, int>> GetCountersAsync(string userId);
    }
}