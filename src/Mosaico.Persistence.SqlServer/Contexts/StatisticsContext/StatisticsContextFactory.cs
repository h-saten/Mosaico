using Mosaico.Core.EntityFramework;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.StatisticsContext
{
    public class StatisticsContextFactory : DbContextFactoryBase<StatisticsContext>, IDbFactory<StatisticsContext>
    {
        protected override string DbSchema => Domain.Statistics.Constants.Schema;
    }
}