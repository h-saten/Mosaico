using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Domain.BusinessManagement.EntityConfigurations
{
    public class CompanyEntityConfiguration : EntityConfigurationBase<Company>
    {
        protected override string TableName => Constants.Tables.Companies;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.CompanyName)
                .IsRequired();
            builder.HasIndex(p => p.Slug).IsUnique();
            builder.HasMany(p => p.TeamMembers).WithOne(m => m.Company).HasForeignKey(m => m.CompanyId);

        }
    }
}