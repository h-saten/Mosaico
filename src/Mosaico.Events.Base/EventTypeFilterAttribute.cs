using System;

namespace Mosaico.Events.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EventTypeFilterAttribute : Attribute
    {
        public EventTypeFilterAttribute(Type type)
        {
            Type = type;
        }
        
        public Type Type { get; }
    }
}