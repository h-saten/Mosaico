using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.DataProtection
{
    public class DataProtectionDbContext : DbContextBase<DataProtectionDbContext>, IDataProtectionKeyContext, IDataProtectionContext
    {
        public DbSet<DataProtectionElement> DataProtectionXmlElements { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        public string ContextName => null;
        public DataProtectionDbContext(DbContextOptions<DataProtectionDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Constants.Schemas.DataProtectionSchema);
            base.OnModelCreating(modelBuilder);
        }
    }
}