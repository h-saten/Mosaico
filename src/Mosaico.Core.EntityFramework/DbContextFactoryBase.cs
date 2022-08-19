using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mosaico.Core.EntityFramework.Configurations;

namespace Mosaico.Core.EntityFramework
{
    public abstract class DbContextFactoryBase<TContext> where TContext : DbContext
    {
        protected string ConnectionString;
        protected virtual string MigrationHistoryTableName => $"_EF_Migrations_{DbSchema}";
        protected virtual string SqlServerConfigurationSectionName => SqlServerConfiguration.SectionName;
        protected abstract string DbSchema { get; }
        
        protected DbContextFactoryBase()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .AddEnvironmentVariables();
            var configuration = configBuilder.Build();
            var sqlSettings = new SqlServerConfiguration();
            configuration.GetSection(SqlServerConfigurationSectionName).Bind(sqlSettings);
            ConnectionString = sqlSettings.ConnectionString;
        }

        protected DbContextFactoryBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public virtual DbContextOptionsBuilder<TContext> GetOptions(DbContextOptions<TContext> options = null)
        {
            var optionsBuilder = options == null ? new DbContextOptionsBuilder<TContext>() : new DbContextOptionsBuilder<TContext>(options);
            optionsBuilder.UseSqlServer(ConnectionString, builder =>
            {
                builder.CommandTimeout(120);
                builder.MigrationsHistoryTable(string.IsNullOrWhiteSpace(MigrationHistoryTableName) ? $"_EF_Migrations_{DbSchema}" : MigrationHistoryTableName, DbSchema);
            });
            return optionsBuilder;
        }

        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public virtual TContext CreateDbContext(string[] args)
        {
            var builder = GetOptions();
            var context = (TContext) Activator.CreateInstance(typeof(TContext), builder.Options, null);
            return context;
        }
    }
}