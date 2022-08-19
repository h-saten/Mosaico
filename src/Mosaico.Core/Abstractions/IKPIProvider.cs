using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Core.Abstractions
{
    public interface IKPIProvider
    {
        Task<List<KeyValuePair<string, string>>> GetKPIsAsync();
    }
}