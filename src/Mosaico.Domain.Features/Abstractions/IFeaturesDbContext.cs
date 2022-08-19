using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Features.Entities;

namespace Mosaico.Domain.Features.Abstractions
{
    public interface IFeaturesDbContext : IDbContext
    {
        DbSet<Feature> Features { get; set; }
        DbSet<BetaTester> BetaTesters { get; set; }
        DbSet<TestSubmission> TestSubmissions { get; set; }
    }
}