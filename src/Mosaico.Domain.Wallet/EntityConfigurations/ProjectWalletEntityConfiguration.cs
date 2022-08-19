using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class ProjectWalletEntityConfiguration : EntityConfigurationBase<ProjectWallet>
    {
        protected override string TableName => Constants.Tables.ProjectWallets;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectWallet> builder)
        {
            base.Configure(builder);
            builder.HasIndex(p => p.ProjectId).IsUnique(true);
        }
    }
}