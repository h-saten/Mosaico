using System;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.SDK.Features.Abstractions;

namespace Mosaico.SDK.Features.Guards
{
    public class FeatureGuard : IFeatureGuard
    {
        private readonly IFeaturesClient _featuresClient;

        public FeatureGuard(IFeaturesClient featuresClient)
        {
            _featuresClient = featuresClient;
        }

        public async Task<bool> CanExecuteAsync(string name, string userId, CancellationToken t = new CancellationToken())
        {
            var feature = await _featuresClient.GetFeatureAsync(name, t);
            if (feature == null)
            {
                return true;
            }

            var tester = await _featuresClient.GetBetaTesterAsync(userId, t);

            return feature.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase) && (feature.IsGloballyAvailable || tester?.IsEnabled == true);
        }
    }
}