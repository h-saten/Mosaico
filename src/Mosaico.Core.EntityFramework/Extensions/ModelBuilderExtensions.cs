using System.Linq;
using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Core.EntityFramework.Encryption;

namespace Mosaico.Core.EntityFramework.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyEncryptionValueConverter(this ModelBuilder modelBuilder, string pswd)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var attributes = property?.PropertyInfo?.GetCustomAttributes(typeof(EncryptedAttribute), false);
                    if (attributes != null)
                    {
                        if (attributes.Any())
                        {
                            property.SetValueConverter(new EncryptionConverter(pswd));
                        }
                    }
                }
            }
        }
    }
}