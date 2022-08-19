using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Extensions;
using System.Collections.Generic;
using Mosaico.Core.EntityFramework.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.BusinessContext
{
    public class BusinessContext : DbContextBase<BusinessContext>, IBusinessDbContext
    {
        public BusinessContext(DbContextOptions<BusinessContext> options, IEnumerable<ISaveChangesCommandInterceptor> saveChangesCommandInterceptor = null) :
            base(options, saveChangesCommandInterceptor)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TeamMemberRole> TeamMemberRoles { get; set; }
        public DbSet<CompanySubscriber> CompanySubscribers { get; set; }
        public DbSet<Verification> Verifications { get; set; }
        public DbSet<Shareholder> Shareholders { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Domain.BusinessManagement.Constants.Schema);
            modelBuilder.ApplyBusinessManagementConfiguration();
            base.OnModelCreating(modelBuilder);
        }

        public string ContextName => "core";
    }
}