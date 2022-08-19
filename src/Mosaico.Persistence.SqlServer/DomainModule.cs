using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Base.Abstractions;
using Mosaico.Core.EntityFramework.Abstractions;
using Mosaico.Core.EntityFramework.Configurations;
using Mosaico.Core.EntityFramework.Interceptors;
using Mosaico.Persistence.SqlServer.Contexts.BusinessContext;
using Mosaico.Persistence.SqlServer.Contexts.DataProtection;
using Mosaico.Persistence.SqlServer.Contexts.DocumentContext;
using Mosaico.Persistence.SqlServer.Contexts.FeaturesContext;
using Mosaico.Persistence.SqlServer.Contexts.ProjectContext;
using Mosaico.Persistence.SqlServer.Contexts.StatisticsContext;
using Mosaico.Persistence.SqlServer.Contexts.VentureFund;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.Persistence.SqlServer.Extensions;

namespace Mosaico.Persistence.SqlServer
{
    public class DomainModule : Module
    {
        private readonly IConfiguration _configuration;

        public DomainModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var sqlServerSettings = new SqlServerConfiguration();
            _configuration.GetSection(SqlServerConfiguration.SectionName).Bind(sqlServerSettings);
            builder.RegisterInstance(sqlServerSettings).Keyed<SqlServerConfiguration>("Domain");
            RegisterDependencies(builder, sqlServerSettings.ConnectionString);
        }

        private static void RegisterDependencies(ContainerBuilder builder, string connectionString)
        {
            builder.RegisterDbContext<ProjectContext, ProjectContextFactory>(connectionString);
            builder.RegisterDbContext<BusinessContext, BusinessContextFactory>(connectionString);
            builder.RegisterDbContext<WalletContext, WalletContextFactory>(connectionString);
            builder.RegisterDbContext<DocumentContext, DocumentContextFactory>(connectionString);
            builder.RegisterDbContext<DataProtectionDbContext, DataProtectionDbContextFactory>(connectionString);
            builder.RegisterDbContext<FeaturesContext, FeaturesContextFactory>(connectionString);
            builder.RegisterDbContext<StatisticsContext, StatisticsContextFactory>(connectionString);
            builder.RegisterDbContext<VentureFundContext, VentureFundContextFactory>(connectionString);
            builder.RegisterType<MigrationRunner>().As<IMigrationRunner>();
            builder.RegisterType<TrackingDataUpdateInterceptor>().As<ISaveChangesCommandInterceptor>();
        }
    }
}