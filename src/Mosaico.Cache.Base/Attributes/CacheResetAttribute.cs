using System;

namespace Mosaico.Cache.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CacheResetAttribute : Attribute
    {
        public CacheResetAttribute(string requestName, string pattern = null)
        {
            RequestName = requestName;
            Pattern = pattern;
        }
        
        public string RequestName { get; private set; }
        public string Pattern { get; private set; }
    }
}