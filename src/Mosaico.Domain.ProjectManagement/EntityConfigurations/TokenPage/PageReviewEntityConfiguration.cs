using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage
{
    public class PageReviewEntityConfiguration : EntityConfigurationBase<PageReview>
    {
        protected override string TableName => Constants.Tables.PageReviews;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<PageReview> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Page).WithMany(p => p.PageReviews).HasForeignKey(t => t.PageId);
            builder.Property(t => t.Link).IsRequired(true);
            builder.Property(t => t.Category).HasDefaultValue(PageReviewCategory.FACEBOOK);
        }
    }
}