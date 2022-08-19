using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.EntityConfigurations.VentureFund;

namespace Mosaico.Domain.Wallet.Extensions
{
    public static class VentureFundModelBuilderExtensions
    {
        public static void ApplyVentureFundModel(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new VentureFundEntityConfiguration());
            builder.ApplyConfiguration(new VentureFundTokenEntityConfiguration());
        }
    }
}