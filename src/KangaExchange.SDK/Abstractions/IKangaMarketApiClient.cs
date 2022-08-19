using System.Collections.Generic;
using System.Threading.Tasks;
using KangaExchange.SDK.Models;

namespace KangaExchange.SDK.Abstractions
{
    public interface IKangaMarketApiClient
    {
        Task<List<KangaMarket>> GetMarketsAsync();
    }
}