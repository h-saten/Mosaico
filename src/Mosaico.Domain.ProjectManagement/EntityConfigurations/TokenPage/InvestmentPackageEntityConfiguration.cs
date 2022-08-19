using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage
{
    public class InvestmentPackageEntityConfiguration : EntityConfigurationBase<InvestmentPackage>
    {
        protected override string TableName => Constants.Tables.InvestmentPackages;
        protected override string Schema => Constants.Schema;
    }
    
    public class InvestmentPackageTranslationEntityConfiguration : EntityConfigurationBase<InvestmentPackageTranslation>
    {
        protected override string TableName => Constants.Tables.InvestmentPackageTranslations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<InvestmentPackageTranslation> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.InvestmentPackage)
                .WithMany(f => f.Translations)
                .HasForeignKey(t => t.EntityId);
        }
    }
}