using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.Statistics.Abstractions;
using Mosaico.Core.Abstractions;

namespace Mosaico.Application.Statistics.Queries.GetKPIs
{
    public class GetKPIsQueryHandler : IRequestHandler<GetKPIsQuery, GetKPIsQueryResponse>
    {
        private readonly IKPIService _kpiService;

        public GetKPIsQueryHandler(IKPIService kpiService)
        {
            _kpiService = kpiService;
        }

        public async Task<GetKPIsQueryResponse> Handle(GetKPIsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetKPIsQueryResponse();
            
            var kpis = await _kpiService.GetKPIsAsync();
            foreach (var kpi in kpis)
            {
                if (!response.ContainsKey(kpi.Key))
                {
                    response.Add(kpi.Key, 0);
                }

                if (decimal.TryParse(kpi.Value, out var kpiValue))
                {
                    response[kpi.Key] = kpiValue;
                }
            }
            return response;
        }
    }
}