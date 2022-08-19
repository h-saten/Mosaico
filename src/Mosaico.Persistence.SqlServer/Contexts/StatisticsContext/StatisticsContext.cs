using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Core.EntityFramework.Abstractions;
using Mosaico.Domain.Statistics.Abstractions;
using Mosaico.Domain.Statistics.Entities;
using Mosaico.Domain.Statistics.Extensions;

namespace Mosaico.Persistence.SqlServer.Contexts.StatisticsContext
{
    public class StatisticsContext : DbContextBase<StatisticsContext>, IStatisticsDbContext
    {
        public StatisticsContext(DbContextOptions<StatisticsContext> options, IEnumerable<ISaveChangesCommandInterceptor> saveChangesCommandInterceptor = null) :
            base(options, saveChangesCommandInterceptor)
        {
        }

        public DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }
        public DbSet<KPI> KPIs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Domain.Statistics.Constants.Schema);
            modelBuilder.ApplyStatisticsConfiguration();
            base.OnModelCreating(modelBuilder);
        }
        
        public string ContextName => "statistics";
    }
}