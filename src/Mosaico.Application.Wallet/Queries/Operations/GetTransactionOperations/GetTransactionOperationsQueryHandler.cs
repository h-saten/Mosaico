using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Operations.GetTransactionOperations
{
    public class GetTransactionOperationsQueryHandler : IRequestHandler<GetTransactionOperationsQuery, GetTransactionOperationsQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;

        public GetTransactionOperationsQueryHandler(IWalletDbContext walletDbContext, IMapper mapper)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
        }

        public async Task<GetTransactionOperationsQueryResponse> Handle(GetTransactionOperationsQuery request, CancellationToken cancellationToken)
        {
            var operationsQuery = _walletDbContext.Operations.OrderByDescending(o => o.CreatedAt).Where(o => o.TransactionId == request.TransactionId);
            var items = await operationsQuery.AsNoTracking().Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
            var count = await operationsQuery.CountAsync(cancellationToken);
            return new GetTransactionOperationsQueryResponse
            {
                Entities = items.Select(e => _mapper.Map<OperationDTO>(e)),
                Total = count
            };
        }
    }
}