using System;

namespace Mosaico.Events.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventInfoAttribute : Attribute
    {
        public EventInfoAttribute(string name, string path)
        {
            Path = path;
            Name = name;
        }

        public (string Topic, string Subscription)? GetSubscriptionDetails()
        {
            if (!string.IsNullOrWhiteSpace(Path) && Path.Contains(":"))
            {
                var parts = Path.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    return (parts[0], parts[1]);
                }
            }

            return null;
        }

        public string Path { get; }
        public string Name { get; }
    }
}