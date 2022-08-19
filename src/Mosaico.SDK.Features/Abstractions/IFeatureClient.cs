using Mosaico.SDK.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.SDK.Features.Abstractions
{
    public interface IFeaturesClient
    {
        Task<MosaicoFeature> GetFeatureAsync(Guid id, CancellationToken token = new CancellationToken());

        Task<List<MosaicoFeature>> GetFeaturesByEntityIdAsync(Guid id, CancellationToken token = new CancellationToken());
        Task<MosaicoFeature> GetFeatureAsync(string name, CancellationToken token = new CancellationToken());
        Task<MosaicoBetaTester> GetBetaTesterAsync(string userId, CancellationToken t = new CancellationToken());
    }
}
