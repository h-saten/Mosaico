using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Features.Entities;

namespace Mosaico.Domain.Features.EntityConfigurations
{
    public class FeaturesEntityConfiguration : EntityConfigurationBase<Feature>
    {
        protected override string TableName => Constants.Tables.Features;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<Feature> builder)
        {
            base.Configure(builder);
            builder.HasIndex(x => x.EntityId).IsUnique(false);
        }
    }
}