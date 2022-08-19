using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Operations.GetUserOperations
{
    public class GetUserOperationsQueryHandler : IRequestHandler<GetUserOperationsQuery, GetUserOperationsQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;

        public GetUserOperationsQueryHandler(IWalletDbContext walletDbContext, IMapper mapper)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
        }

        public async Task<GetUserOperationsQueryResponse> Handle(GetUserOperationsQuery request, CancellationToken cancellationToken)
        {
            var operationsQuery = _walletDbContext.Operations.OrderByDescending(o => o.CreatedAt).Where(o => o.UserId == request.UserId);
            var items = await operationsQuery.AsNoTracking().Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
            var count = await operationsQuery.CountAsync(cancellationToken);
            return new GetUserOperationsQueryResponse
            {
                Entities = items.Select(e => _mapper.Map<OperationDTO>(e)),
                Total = count
            };
        }
    }
}