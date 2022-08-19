using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Integration.SignalR.Abstractions
{
    public interface ICountersDispatcher
    {
        Task DispatchCounterAsync(string userId, KeyValuePair<string, int> counter);
    }
}