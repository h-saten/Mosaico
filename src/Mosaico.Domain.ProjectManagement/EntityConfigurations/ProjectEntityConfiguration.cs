using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class ProjectEntityConfiguration : EntityConfigurationBase<Project>
    {
        protected override string TableName => Constants.Tables.Projects;
        protected override string Schema => Constants.Schema;
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Status).WithMany().HasForeignKey(p => p.StatusId);
            builder.HasIndex(p => p.Slug).IsUnique(true);
            builder.HasIndex(p => p.SlugInvariant).IsUnique(true);
            builder.HasIndex(p => p.TitleInvariant).IsUnique(true);
            builder.HasMany(p => p.Members).WithOne(m => m.Project).HasForeignKey(m => m.ProjectId);
            builder.HasOne(c => c.Crowdsale)
                .WithOne(p => p.Project).HasForeignKey<Project>(p => p.CrowdsaleId).IsRequired(false);
            builder.HasOne(c => c.ProjectAffiliation)
                .WithOne(p => p.Project).HasForeignKey<Project>(p => p.ProjectAffiliationId).IsRequired(false);
            builder
                .HasMany(p => p.PaymentMethods)
                .WithMany(p => p.Projects)
                .UsingEntity<ProjectPaymentMethod>(
                    j => j
                        .HasOne(pt => pt.PaymentMethod)
                        .WithMany(t => t.ProjectPaymentMethods)
                        .HasForeignKey(pt => pt.PaymentMethodId),
                    j => j
                        .HasOne(pt => pt.Project)
                        .WithMany(p => p.ProjectPaymentMethods)
                        .HasForeignKey(pt => pt.ProjectId),
                    j =>
                    {
                        j.HasKey(t => new { t.ProjectId, t.PaymentMethodId });
                    });
        }
    }
}