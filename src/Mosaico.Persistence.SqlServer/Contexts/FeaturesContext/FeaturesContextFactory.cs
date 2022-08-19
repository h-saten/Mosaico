using Mosaico.Core.EntityFramework;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.FeaturesContext
{
    public class FeaturesContextFactory : DbContextFactoryBase<FeaturesContext>, IDbFactory<FeaturesContext>
    {
        protected override string DbSchema => Domain.Features.Constants.Schema;
    }
}