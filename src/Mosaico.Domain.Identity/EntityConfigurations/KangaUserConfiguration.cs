using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class KangaUserConfiguration : EntityConfigurationBase<KangaUser>
    {
        protected override string TableName => Constants.Tables.KangaUser;
        protected override string Schema => Constants.Schema;
    
        public override void Configure(EntityTypeBuilder<KangaUser> builder)
        {
            base.Configure(builder);
            builder.HasIndex(p => p.Email).IsUnique(true);
        }
    }
}