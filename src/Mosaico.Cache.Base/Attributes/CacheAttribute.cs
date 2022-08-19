using System;

namespace Mosaico.Cache.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CacheAttribute : Attribute
    {
        public string Pattern { get; private set; }
        public CacheAttribute(string pattern = null)
        {
            Pattern = pattern;
        }
        // Name of the property, which value will act as a key to store in distributed cache
        public int ExpirationInMinutes { get; set; }
    }
}