using System;
using System.Collections.Generic;
using System.Linq;
using Mosaico.SDK.Identity.Models;

namespace Mosaico.SDK.Identity.Extensions
{
    public static class MosaicoPermissionsExtensions
    {
        public static bool HasPermission(this List<MosaicoPermission> permissions, string key, Guid? entityId = null)
        {
            if (permissions != null && !string.IsNullOrWhiteSpace(key))
            {
                return entityId.HasValue ? 
                    permissions.Any(p => p.Key == key && p.EntityId.HasValue && p.EntityId.Value == entityId.Value) : 
                    permissions.Any(p => p.Key == key && !p.EntityId.HasValue);
            }

            return false;
        }
    }
}