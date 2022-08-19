using Mosaico.Core.EntityFramework;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.Identity
{
    public class ApplicationDbContextFactory : DbContextFactoryBase<ApplicationDbContext>, IDbFactory<ApplicationDbContext>
    {
        protected override string SqlServerConfigurationSectionName => Domain.Identity.Constants.IdentityServerDbConfigurationSection;
        protected override string MigrationHistoryTableName => "__EFMigrationsHistory";

        protected override string DbSchema => "dbo";
    }
}