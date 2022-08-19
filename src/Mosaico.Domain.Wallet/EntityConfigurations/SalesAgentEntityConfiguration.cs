using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class SalesAgentEntityConfiguration : EntityConfigurationBase<SalesAgent>
    {
        protected override string TableName => Constants.Tables.SalesAgents;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<SalesAgent> builder)
        {
            base.Configure(builder);
        }
    }
}