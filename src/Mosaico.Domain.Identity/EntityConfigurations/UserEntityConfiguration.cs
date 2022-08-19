using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasOne(u => u.DeletionRequest).WithOne(u => u.User).HasForeignKey<DeletionRequest>(u => u.UserId).IsRequired(false);
            builder.Property(t => t.AMLStatus).HasDefaultValue(AMLStatus.Unknown);
        }
    }
}