using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class ProjectPaymentMethodEntityConfiguration : IEntityTypeConfiguration<ProjectPaymentMethod>
    {
        private string TableName => Constants.Tables.ProjectPaymentMethods;
        private string Schema => Constants.Schema;
        
        public virtual void Configure(EntityTypeBuilder<ProjectPaymentMethod> builder)
        {
            builder.ToTable(TableName,Schema);
        }
    }
}