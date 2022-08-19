using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class DeletionRequestEntityConfiguration : EntityConfigurationBase<DeletionRequest>
    {

        protected override string TableName => Constants.Tables.DeletionRequests;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<DeletionRequest> builder)
        {
            base.Configure(builder);
            builder.HasOne(u => u.User).WithOne(u => u.DeletionRequest).HasForeignKey<DeletionRequest>(u => u.UserId).IsRequired(false);
        }
    }
}