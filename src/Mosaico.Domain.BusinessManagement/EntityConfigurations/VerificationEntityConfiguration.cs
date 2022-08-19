using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Domain.BusinessManagement.EntityConfigurations
{
    public class VerificationEntityConfiguration : EntityConfigurationBase<Verification>
    {
        protected override string TableName => Constants.Tables.Verification;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<Verification> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.CompanyAddressUrl).IsRequired();
            builder.Property(p => p.CompanyRegistrationUrl).IsRequired();
            builder.HasMany(p => p.Shareholders).WithOne(m => m.Verification).HasForeignKey(m => m.VerificationId);
        }
    }
}