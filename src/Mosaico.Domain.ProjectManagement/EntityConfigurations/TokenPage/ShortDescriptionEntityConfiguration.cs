using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage
{
    public class ShortDescriptionEntityConfiguration : EntityConfigurationBase<ShortDescription>
    {
        protected override string TableName => Constants.Tables.ShortDescription;
        protected override string Schema => Constants.Schema;
    }

    public class ShortDescriptionTranslationEntityConfiguration : EntityConfigurationBase<ShortDescriptionTranslation>
    {
        protected override string TableName => Constants.Tables.ShortDescriptionTranslations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ShortDescriptionTranslation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.ShortDescription)
                .WithMany(f => f.Translations)
                .HasForeignKey(t => t.EntityId);
        }
    }
}