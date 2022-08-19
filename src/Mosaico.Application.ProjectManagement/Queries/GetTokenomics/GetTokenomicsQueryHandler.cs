using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.SDK.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetTokenomics
{
    public class GetTokenomicsQueryHandler : IRequestHandler<GetTokenomicsQuery, GetTokenomicsQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private readonly IWalletClient _walletClient;
 
        public GetTokenomicsQueryHandler(IProjectDbContext projectDbContext, IWalletClient walletClient, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _walletClient = walletClient;
            _logger = logger;
        }

        public async Task<GetTokenomicsQueryResponse> Handle(GetTokenomicsQuery request, CancellationToken cancellationToken)
        {
            var project =
                await _projectDbContext.Projects.Include(t => t.Stages)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == request.ProjectId, cancellationToken);
            
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var response = new GetTokenomicsQueryResponse();
            
            foreach (var stage in project.Stages.OrderBy(t => t.StartDate))
            {
                response.Labels.Add(stage.Name);
                response.Series.Add(stage.TokensSupply);
            }

            if (project.TokenId.HasValue)
            {
                var token = await _walletClient.GetTokenAsync(project.TokenId.Value);
                if (token != null && token.TotalSupply > 0)
                {
                    var tokenDistribution = await _walletClient.GetTokenDistributionAsync(project.TokenId.Value);
                    if (tokenDistribution != null && tokenDistribution.Any())
                    {
                        response.Labels.AddRange(tokenDistribution.Select(t => t.Name));
                        response.Series.AddRange(tokenDistribution.Select(t => t.TokenAmount));
                    }
                    //TODO: to configurable feature
                    //response.Labels.Add("MOS Fund");
                    //response.Series.Add(token.TotalSupply * 0.01M);
                    
                    var reserveFund = token.TotalSupply - response.Series.Sum();
                    if (reserveFund > 0)
                    {
                        response.Labels.Add("Reserve");
                        response.Series.Add(reserveFund);
                    }

                    foreach (var series in response.Series)
                    {
                        var percent = series * 100 / token.TotalSupply;
                        response.Percentage.Add(percent);
                    }
                }
            }
            return response;
        }
    }
}