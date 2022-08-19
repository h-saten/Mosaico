using System;
using Mosaico.Cache.Base;

namespace Mosaico.Infrastructure.Redis.Tests.Helpers
{
    public class TestCacheItem : CacheItemBase
    {
        public DateTimeOffset CreatedAt { get; set; }
    }
}