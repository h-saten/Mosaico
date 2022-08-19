using Autofac;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Microsoft.Extensions.Configuration;
using Mosaico.Base.Abstractions;
using Mosaico.Core.EntityFramework.Abstractions;
using Mosaico.Core.EntityFramework.Configurations;
using Mosaico.Core.EntityFramework.Interceptors;
using Mosaico.Domain.Identity;
using Mosaico.Persistence.SqlServer.Contexts.DataProtection;
using Mosaico.Persistence.SqlServer.Contexts.Identity;
using Mosaico.Persistence.SqlServer.Contexts.PersistedGrant;
using Mosaico.Persistence.SqlServer.Extensions;

namespace Mosaico.Persistence.SqlServer
{
    public class IdentityDomainModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly string _configurationSectionName;

        public IdentityDomainModule(IConfiguration configuration, string configurationSectionName = Domain.Identity.Constants.IdentityServerDbConfigurationSection)
        {
            _configuration = configuration;
            _configurationSectionName = configurationSectionName;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var sqlServerSettings = new SqlServerConfiguration();
            _configuration.GetSection(_configurationSectionName).Bind(sqlServerSettings);
            builder.RegisterInstance(sqlServerSettings).Keyed<SqlServerConfiguration>("Identity");
            RegisterDependencies(builder, sqlServerSettings.ConnectionString);
        }

        private static void RegisterDependencies(ContainerBuilder builder, string connectionString)
        {
            builder.RegisterModule<IdentityModule>();
            builder.RegisterDbContext<ApplicationDbContext, ApplicationDbContextFactory>(connectionString);
            builder.RegisterDbContext<IdentityPersistedGrantDbContext, IdentityPersistedGrantDbContextFactory>(connectionString)
                .As<IPersistedGrantDbContext>();
            builder.RegisterDbContext<DataProtectionDbContext, DataProtectionDbContextFactory>(connectionString);
            builder.RegisterType<MigrationRunner>().As<IMigrationRunner>();
            builder.RegisterType<TrackingDataUpdateInterceptor>().As<ISaveChangesCommandInterceptor>();
        }
    }
}