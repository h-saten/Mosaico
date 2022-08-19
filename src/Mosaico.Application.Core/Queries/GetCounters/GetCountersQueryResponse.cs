using System.Collections.Generic;

namespace Mosaico.Application.Core.Queries.GetCounters
{
    public class GetCountersQueryResponse
    {
        public List<KeyValuePair<string, int>> Counters { get; set; } = new List<KeyValuePair<string, int>>();
    }
}