using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.Ratings;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class ProjectLikeEntityConfiguration : EntityConfigurationBase<ProjectLike>
    {
        protected override string TableName => Constants.Tables.ProjectLikes;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectLike> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Project).WithMany(p => p.Likes).HasForeignKey(t => t.ProjectId);
        }
    }
}