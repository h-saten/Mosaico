using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class PermissionEntityConfiguration : EntityConfigurationBase<Permission>
    {
        protected override string TableName => Constants.Tables.Permissions;
        protected override string Schema => Constants.Schema;
    
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);
            builder.HasIndex(p => p.Key).IsUnique(true);
            builder.HasMany(p => p.Users).WithOne(u => u.Permission).HasForeignKey(p => p.PermissionId);
        }
    }
}