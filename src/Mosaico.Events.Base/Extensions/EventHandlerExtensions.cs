using System.Reflection;

namespace Mosaico.Events.Base.Extensions
{
    public static class EventHandlerExtensions
    {
        public static string GetFilterType(this IEventHandler handler)
        {
            if (handler.GetType().GetCustomAttribute(typeof(EventTypeFilterAttribute)) is EventTypeFilterAttribute attribute)
            {
                return attribute.Type.FullName;
            }

            return null;
        }
    }
}