using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Features.Abstractions;
using Mosaico.Domain.Features.Entities;
using Mosaico.Domain.Features.Exceptions;

namespace Mosaico.Domain.Features.Extensions
{
    public static class FeaturesDbContextExtensions
    {
        public static async Task<Feature> GetFeatureOrThrowAsync(this IFeaturesDbContext context, string identifier, CancellationToken tok = new CancellationToken())
        {
            Feature feature;
            if (Guid.TryParse(identifier, out var featureId))
            {
                feature = await context.Features.FirstOrDefaultAsync(p => p.Id == featureId, tok);
            }
            else
            {
                feature = await context.Features.FirstOrDefaultAsync(p => p.FeatureName == identifier, tok);
            }

            if (feature == null)
            {
                throw new FeatureNotFoundException(identifier);
            }

            return feature;
        }
    }
}