using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class DeflationEntityConfiguration : EntityConfigurationBase<Deflation>
    {
        protected override string TableName => Constants.Tables.Deflations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Deflation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Token).WithOne(t => t.Deflation)
                .HasForeignKey<Deflation>(t => t.TokenId);
        }
    }
}