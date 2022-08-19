using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Features.Abstractions;
using Mosaico.Domain.Features.Entities;
using Mosaico.Domain.Features.Extensions;
using System.Collections.Generic;
using Mosaico.Core.EntityFramework.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.FeaturesContext
{
    public class FeaturesContext : DbContextBase<FeaturesContext>, IFeaturesDbContext
    {
        public FeaturesContext(DbContextOptions<FeaturesContext> options, IEnumerable<ISaveChangesCommandInterceptor> saveChangesCommandInterceptor = null) :
            base(options, saveChangesCommandInterceptor)
        {
        }

        public DbSet<Feature> Features { get; set; }
        public DbSet<BetaTester> BetaTesters { get; set; }
        public DbSet<TestSubmission> TestSubmissions { get; set; }
        public string ContextName => "core";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Domain.Features.Constants.Schema);
            modelBuilder.ApplyFeaturesConfiguration();
            base.OnModelCreating(modelBuilder);
        }
    }
}