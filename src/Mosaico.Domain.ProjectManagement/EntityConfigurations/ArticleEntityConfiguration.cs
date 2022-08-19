using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class ArticleEntityConfiguration : EntityConfigurationBase<Article>
    {
        protected override string TableName => Constants.Tables.Articles;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Article> builder)
        {
            base.Configure(builder);
            builder.HasOne(d => d.Project).WithMany(p => p.Articles).HasForeignKey(t => t.ProjectId);
        }
    }
}