using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class PhoneNumberConfirmationCodeEntityConfiguration : EntityConfigurationBase<PhoneNumberConfirmationCode>
    {
        protected override string TableName => Constants.Tables.PhoneNumberConfirmationCodes;
        protected override string Schema => "dbo";
        public override void Configure(EntityTypeBuilder<PhoneNumberConfirmationCode> builder)
        {
            base.Configure(builder);
            builder.HasOne(u => u.User).WithMany().HasForeignKey(c => c.UserId);
            builder.OwnsOne(u => u.PhoneNumber, p =>
            {
                p.Property(x => x.Value).HasColumnName("PhoneNumber");
            });
        }
    }
}