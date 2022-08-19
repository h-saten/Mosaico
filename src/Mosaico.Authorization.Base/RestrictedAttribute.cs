using System;
using System.Collections.Generic;
using System.Linq;

namespace Mosaico.Authorization.Base
{
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RestrictedAttribute : Attribute
    {
        public RestrictedAttribute(string propertyName, string permission)
        {
            RestrictedPropertyName = propertyName;
            Permission = permission;
        }
        
        public RestrictedAttribute(params string[] permissions)
        {
            Permissions = permissions.ToList();
        }
        
        public string RestrictedPropertyName { get; }
        public string Permission { get; }
        public List<string> Permissions { get; }
    }
}