using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Domain.BusinessManagement.EntityConfigurations
{
    public class ShareholderEntityConfiguration : EntityConfigurationBase<Shareholder>
    {
        protected override string TableName => Constants.Tables.Shareholder;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<Shareholder> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.FullName).IsRequired();
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.Share).IsRequired();
            builder.HasOne(s => s.Verification).WithMany(s => s.Shareholders).HasForeignKey(s => s.VerificationId);
        }
    }
}