using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Statistics.EntityConfiguration;

namespace Mosaico.Domain.Statistics.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyStatisticsConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PurchaseTransactionEntityConfiguration());
            builder.ApplyConfiguration(new KPIEntityConfiguration());
        }
    }
}