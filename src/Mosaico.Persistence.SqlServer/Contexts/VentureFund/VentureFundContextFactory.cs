using Mosaico.Core.EntityFramework;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.VentureFund
{
    public class VentureFundContextFactory : DbContextFactoryBase<VentureFundContext>, IDbFactory<VentureFundContext>
    {
        protected override string DbSchema => Domain.Wallet.Constants.FundSchema;
    }
}