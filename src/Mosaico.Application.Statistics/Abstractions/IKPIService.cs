using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Application.Statistics.Abstractions
{
    public interface IKPIService
    {
        Task<List<KeyValuePair<string, string>>> GetKPIsAsync();
        Task CreateOrUpdateKPIAsync(string key, string value);
    }
}