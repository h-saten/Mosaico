using Mosaico.Cache.Base;

namespace Mosaico.Application.Wallet.Caching
{
    public class CompanyNativeBalanceCacheItem : CacheItemBase
    {
        public decimal Balance { get; set; }
    }
}