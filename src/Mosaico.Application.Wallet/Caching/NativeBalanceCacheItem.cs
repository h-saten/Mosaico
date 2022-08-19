using Mosaico.Cache.Base;

namespace Mosaico.Application.Wallet.Caching
{
    public class NativeBalanceCacheItem : CacheItemBase
    {
        public decimal Balance { get; set; }
    }
}