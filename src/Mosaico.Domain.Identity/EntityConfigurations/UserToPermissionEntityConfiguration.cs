using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class UserToPermissionEntityConfiguration : EntityConfigurationBase<UserToPermission>
    {
        protected override string TableName => Constants.Tables.UserToPermission;
        protected override string Schema => Constants.Schema;
    
        public override void Configure(EntityTypeBuilder<UserToPermission> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Permission).WithMany(p => p.Users).HasForeignKey(p => p.PermissionId);
            builder.HasOne(p => p.User).WithMany(p => p.Permissions).HasForeignKey(p => p.UserId);
        }
    }
}