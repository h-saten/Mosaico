using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class NFTCollectionEntityConfiguration : EntityConfigurationBase<NFTCollection>
    {
        protected override string TableName => Constants.Tables.NFTCollections;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<NFTCollection> builder)
        {
            base.Configure(builder);
            
        }
    }
}