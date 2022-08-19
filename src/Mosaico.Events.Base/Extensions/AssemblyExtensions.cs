using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mosaico.Events.Base.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypesWithEventAttribute(this Assembly assembly)
        {
            return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(EventInfoAttribute), true).Length > 0);
        }
    }
}