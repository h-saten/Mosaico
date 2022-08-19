using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Features.Abstractions;
using Mosaico.Domain.Features.Entities;
using Mosaico.SDK.Base.Exceptions;
using Mosaico.SDK.Features.Abstractions;
using Mosaico.SDK.Features.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.SDK.Features
{
    public class FeaturesClient : IFeaturesClient
    {
        private readonly IFeaturesDbContext _context;
        private readonly ILogger _logger;

        public FeaturesClient(IFeaturesDbContext context, ILogger logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MosaicoBetaTester> GetBetaTesterAsync(string userId, CancellationToken t = new CancellationToken())
        {
            var tester = await _context.BetaTesters.FirstOrDefaultAsync(t => t.UserId == userId, t);
            if (tester == null)
            {
                return null;
            }

            return new MosaicoBetaTester
            {
                Id = tester.Id,
                Type = tester.Type,
                EnrolledAt = tester.EnrolledAt,
                IsEnabled = tester.IsEnabled,
                UserId = tester.UserId
            };
        }

        public async Task<MosaicoFeature> GetFeatureAsync(string name, CancellationToken token = new CancellationToken())
        {
            var feature = await _context.Features.FirstOrDefaultAsync(f => f.FeatureName == name && f.EntityId == null, token);
            if (feature == null)
            {
                return null;
            }
            return new MosaicoFeature
            {
                EntityId = feature.EntityId,
                FeatureName = feature.FeatureName,
                Value = feature.Value,
                IsGloballyAvailable = feature.IsGloballyAvailable
            };
        }

        public async Task<MosaicoFeature> GetFeatureAsync(Guid id, CancellationToken token = new CancellationToken())
        {
            var feature = await _context.Features.FirstOrDefaultAsync(p => p.Id == id, cancellationToken: token);
            if (feature == null)
            {
                throw new MosaicoException($"Feature {id} not found", "FEATURE_NOT_FOUND", 404);
            }
            return new MosaicoFeature
            {
                EntityId = feature.EntityId,
                FeatureName = feature.FeatureName,
                Value = feature.Value
            };
        }

        public async Task<List<MosaicoFeature>> GetFeaturesByEntityIdAsync(Guid id, CancellationToken token = new CancellationToken())
        {
            var features = await _context.Features.Where(p => p.EntityId == id).ToListAsync(cancellationToken: token);
            var result = new List<MosaicoFeature>();
            foreach(var f in features)
            {
                result.Add(new MosaicoFeature
                {
                    EntityId = f.EntityId,
                    FeatureName = f.FeatureName,
                    Value = f.Value
                });
            }
            return result;
        }
    }
}
