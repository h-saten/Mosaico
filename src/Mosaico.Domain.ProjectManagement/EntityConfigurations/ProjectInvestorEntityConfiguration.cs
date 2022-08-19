using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class ProjectInvestorEntityConfiguration : EntityConfigurationBase<ProjectInvestor>
    {
        protected override string TableName => Constants.Tables.ProjectInvestors;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<ProjectInvestor> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Stage).WithMany(p => p.ProjectInvestors).HasForeignKey(p => p.StageId);
        }
    }
}