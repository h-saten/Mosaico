using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.PersistedGrant
{
    public class IdentityPersistedGrantDbContextFactory : DbContextFactoryBase<IdentityPersistedGrantDbContext>, IDbFactory<IdentityPersistedGrantDbContext>
    {
        protected override string SqlServerConfigurationSectionName => Domain.Identity.Constants.IdentityServerDbConfigurationSection;
        protected override string MigrationHistoryTableName => "__EFMigrationsHistory";
        protected override string DbSchema => "dbo";

        public override IdentityPersistedGrantDbContext CreateDbContext(string[] args)
        {
            var options = GetOptions();
            var storeOptions = new OperationalStoreOptions
            {
                ConfigureDbContext = dbContextBuilder =>
                    dbContextBuilder.UseSqlServer(ConnectionString),
                EnableTokenCleanup = true,
                TokenCleanupInterval = 3600
            };
            return new IdentityPersistedGrantDbContext(options.Options,
                storeOptions);
        }
    }
}