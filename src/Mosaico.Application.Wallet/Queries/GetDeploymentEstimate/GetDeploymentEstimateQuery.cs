using System.Collections.Generic;
using MediatR;
using Mosaico.Cache.Base.Attributes;
using Mosaico.Integration.SignalR.DTO;

namespace Mosaico.Application.Wallet.Queries.GetDeploymentEstimate
{
    [Cache(ExpirationInMinutes = 3)]
    public class GetDeploymentEstimateQuery : IRequest<List<DeploymentEstimateDTO>>
    {
        
    }
}