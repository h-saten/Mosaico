using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Base.Extensions
{
    public static class DictionaryExtension
    {
        public static async Task<TValue> GetOrCreateAsync<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<Task<TValue>> getValue) 
            where TValue : new()
        {
            if (!dict.TryGetValue(key, out TValue val))
            {
                val = await getValue();
                dict.Add(key, val);
            }

            return val;
        }
    }
}