using System.Linq;
using StackExchange.Redis;

namespace Mosaico.Infrastructure.Redis.Extensions
{
    public static class NameValueEntryExtensions
    {
        public static string GetValue(this NameValueEntry[] values, string name) {
            if (values != null && !string.IsNullOrWhiteSpace(name))
            {
                var valuePair = values.FirstOrDefault(n => n.Name == name);
                return valuePair.Value.ToString();
            }
            return null;
        }
    }
}