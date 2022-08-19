using Mosaico.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    internal class NewsletterSubscribersConfiguration : EntityConfigurationBase<NewsletterSubscribers>
    {
        protected override string TableName => Constants.Tables.NewsletterSubscribers;
        protected override string Schema => "dbo";

        public override void Configure(EntityTypeBuilder<NewsletterSubscribers> builder)
        {
            base.Configure(builder);
            builder.HasIndex(n => n.Email).IsUnique();
        }
    }
}
