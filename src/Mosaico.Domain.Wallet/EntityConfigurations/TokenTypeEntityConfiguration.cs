using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Domain.Wallet.EntityConfigurations
{
    public class TokenTypeEntityConfiguration : EntityConfigurationBase<TokenType>
    {
        protected override string TableName => Constants.Tables.TokenTypes;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<TokenType> builder)
        {
            base.Configure(builder);
            builder.HasIndex(s => s.Key).IsUnique();
            builder.HasData(new TokenType(Constants.TokenType.Utility, "Utility"){Id = new Guid("cddc16c2-5969-4160-ac94-67e57fe8c181")});
            builder.HasData(new TokenType(Constants.TokenType.Security, "Security"){Id = new Guid("062ef44f-592c-4cdc-a1e8-6b2b2521ed16")});
            builder.HasData(new TokenType(Constants.TokenType.NFT, "NFT"){Id = new Guid("1c12c473-d4f7-4906-9700-b6ec8d2b7437")});
        }
    }
}