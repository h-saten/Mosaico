using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class NFTEntityConfiguration : EntityConfigurationBase<NFT>
    {
        protected override string TableName => Constants.Tables.NFTs;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<NFT> builder)
        {
            base.Configure(builder);
            builder.HasOne(n => n.NFTCollection).WithMany(nc => nc.NFTs).HasForeignKey(n => n.NFTCollectionId);
        }
    }
}