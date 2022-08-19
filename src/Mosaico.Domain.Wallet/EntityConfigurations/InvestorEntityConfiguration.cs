using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class InvestorEntityConfiguration : EntityConfigurationBase<Investor>
    {
        protected override string TableName => Constants.Tables.Investors;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Investor> builder)
        {
            base.Configure(builder);
            builder.Property("_balances");
            builder.HasIndex(t => new {t.ProjectId, t.UserId}).IsUnique(true);
        }
    }
}