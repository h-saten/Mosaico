using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class ProjectWalletAccountEntityConfiguration : EntityConfigurationBase<ProjectWalletAccount>
    {
        protected override string TableName => Constants.Tables.ProjectWalletAccounts;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectWalletAccount> builder)
        {
            base.Configure(builder);
            builder.HasOne(a => a.ProjectWallet).WithMany(p => p.Accounts)
                .HasForeignKey(t => t.ProjectWalletId);
            builder.HasIndex(a => new {a.ProjectWalletId, a.UserId});
        }
    }
}