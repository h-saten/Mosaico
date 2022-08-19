using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Domain.BusinessManagement.Abstractions
{
    public interface IBusinessDbContext : IDbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Verification> Verifications { get; set; }
        public DbSet<Shareholder> Shareholders { get; set; }
        public DbSet<TeamMemberRole> TeamMemberRoles { get; set; }
        public DbSet<CompanySubscriber> CompanySubscribers { get; set; }
        public DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}