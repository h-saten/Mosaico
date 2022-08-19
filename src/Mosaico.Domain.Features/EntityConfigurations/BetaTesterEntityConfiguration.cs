using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Features.Entities;

namespace Mosaico.Domain.Features.EntityConfigurations
{
    public class BetaTesterEntityConfiguration : EntityConfigurationBase<BetaTester>
    {
        protected override string TableName => Constants.Tables.BetaTesters;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<BetaTester> builder)
        {
            base.Configure(builder);
            builder.HasIndex(b => b.UserId).IsUnique(true);
            builder.HasMany(b => b.TestSubmissions).WithOne(s => s.BetaTester).HasForeignKey(s => s.BetaTesterId);
        }
    }
}