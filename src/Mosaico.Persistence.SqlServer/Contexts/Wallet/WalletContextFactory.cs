using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.Wallet
{
    public class WalletContextFactory : DbContextFactoryBase<WalletContext>, IDbFactory<WalletContext>
    {
        protected override string DbSchema => Domain.Wallet.Constants.Schema;
    }
}