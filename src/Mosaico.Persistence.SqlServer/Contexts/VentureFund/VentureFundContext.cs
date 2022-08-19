using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Core.EntityFramework.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Fund;
using Mosaico.Domain.Wallet.Extensions;

namespace Mosaico.Persistence.SqlServer.Contexts.VentureFund
{
    public class VentureFundContext : DbContextBase<VentureFundContext>, IVentureFundDbContext
    {
        public VentureFundContext(DbContextOptions<VentureFundContext> options, IEnumerable<ISaveChangesCommandInterceptor> saveChangesCommandInterceptor = null) : base(options, saveChangesCommandInterceptor)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Domain.Wallet.Constants.FundSchema);
            modelBuilder.ApplyVentureFundModel();
            base.OnModelCreating(modelBuilder);
        }

        public string ContextName => "core";
        
        public DbSet<Domain.Wallet.Entities.Fund.VentureFund> VentureFunds { get; set; }
        public DbSet<VentureFundToken> VentureFundTokens { get; set; }
    }
}