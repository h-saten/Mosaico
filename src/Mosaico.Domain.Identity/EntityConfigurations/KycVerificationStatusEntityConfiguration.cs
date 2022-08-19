using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class KycVerificationStatusEntityConfiguration : EntityConfigurationBase<KycVerification>
    {
        protected override string TableName => Constants.Tables.KycVerifications;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<KycVerification> builder)
        {
            base.Configure(builder);
            
        }
    }
}