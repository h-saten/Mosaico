using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TokenLockEntityConfiguration : EntityConfigurationBase<TokenLock>
    {
        protected override string TableName => Constants.Tables.TokenLocks;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<TokenLock> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Token).WithMany().HasForeignKey(t => t.TokenId);
        }
    }
}