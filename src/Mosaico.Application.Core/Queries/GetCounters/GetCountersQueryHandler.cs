using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Core.Abstractions;

namespace Mosaico.Application.Core.Queries.GetCounters
{
    public class GetCountersQueryHandler : IRequestHandler<GetCountersQuery, GetCountersQueryResponse>
    {
        private readonly IEnumerable<ICounterProvider> _counterProviders = null;
        private readonly ICurrentUserContext _userContext;
        
        public GetCountersQueryHandler(ICurrentUserContext userContext, IEnumerable<ICounterProvider> counterProviders = null)
        {
            _userContext = userContext;
            _counterProviders = counterProviders;
        }

        public async Task<GetCountersQueryResponse> Handle(GetCountersQuery request, CancellationToken cancellationToken)
        {
            var response = new GetCountersQueryResponse();
            if (_counterProviders != null)
            {
                foreach (var provider in _counterProviders)
                {
                    var counter = await provider.GetCountersAsync(_userContext.UserId);
                    if (!response.Counters.Any(c => c.Key == counter.Key))
                    {
                        response.Counters.Add(counter);
                    }
                }
            }

            return response;
        }
    }
}