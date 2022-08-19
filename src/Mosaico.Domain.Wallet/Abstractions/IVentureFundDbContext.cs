using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities.Fund;

namespace Mosaico.Domain.Wallet.Abstractions
{
    public interface IVentureFundDbContext  : IDbContext
    {
        DbSet<VentureFund> VentureFunds { get; set; }
        DbSet<VentureFundToken> VentureFundTokens { get; set; }
    }
}