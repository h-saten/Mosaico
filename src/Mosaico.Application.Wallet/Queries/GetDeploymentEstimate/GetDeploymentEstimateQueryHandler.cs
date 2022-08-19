using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Integration.SignalR.DTO;

namespace Mosaico.Application.Wallet.Queries.GetDeploymentEstimate
{
    public class GetDeploymentEstimateQueryHandler : IRequestHandler<GetDeploymentEstimateQuery, List<DeploymentEstimateDTO>>
    {
        private readonly ITimeSeriesRepository _seriesRepository;

        public GetDeploymentEstimateQueryHandler(ITimeSeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }

        public async Task<List<DeploymentEstimateDTO>> Handle(GetDeploymentEstimateQuery request, CancellationToken cancellationToken)
        {
            var estimates = await _seriesRepository.GetLastOrDefaultAsync<List<DeploymentEstimateDTO>>(Domain.Wallet.Constants.RedisKeys.Estimates);
            return estimates;
        }
    }
}