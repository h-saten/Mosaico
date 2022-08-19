using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Statistics.Entities;

namespace Mosaico.Domain.Statistics.Abstractions
{
    public interface IStatisticsDbContext : IDbContext
    {
        DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }
        DbSet<KPI> KPIs { get; set; }
    }
}