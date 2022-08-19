using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations.TokenPage
{
    public class ScriptEntityConfiguration : EntityConfigurationBase<Script>
    {
        protected override string TableName => Constants.Tables.Scripts;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Script> builder)
        {
            base.Configure(builder);
            builder.HasOne(s => s.Page).WithMany(p => p.Scripts).HasForeignKey(t => t.PageId);
        }
    }
}