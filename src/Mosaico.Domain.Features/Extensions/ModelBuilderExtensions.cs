using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Features.EntityConfigurations;

namespace Mosaico.Domain.Features.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyFeaturesConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FeaturesEntityConfiguration());
            builder.ApplyConfiguration(new TestSubmissionEntityConfiguration());
            builder.ApplyConfiguration(new BetaTesterEntityConfiguration());
        }
    }
}