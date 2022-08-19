using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class SecurityCodeEntityConfiguration : EntityConfigurationBase<SecurityCode>
    {
        protected override string TableName => Constants.Tables.SecurityCodes;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<SecurityCode> builder)
        {
            base.Configure(builder);
            builder.HasOne(u => u.User).WithMany().HasForeignKey(c => c.UserId);
            builder.Property(p => p.Context).IsRequired(true);
            builder.Property(p => p.Code).IsRequired(true);
        }
    }
}