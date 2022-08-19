using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class OperationEntityConfiguration : EntityConfigurationBase<Operation>
    {
        protected override string TableName => Constants.Tables.Operations;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Operation> builder)
        {
            base.Configure(builder);
        }
    }
}