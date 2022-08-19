using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.GetPaymentCurrencies
{
    public class GetPaymentCurrenciesHandler : IRequestHandler<GetPaymentCurrenciesQuery, List<PaymentCurrencyDTO>>
    {
        private readonly IWalletDbContext _context;
        private readonly IMapper _mapper;
        
        public GetPaymentCurrenciesHandler(IWalletDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PaymentCurrencyDTO>> Handle(GetPaymentCurrenciesQuery request, CancellationToken cancellationToken)
        {
            return await _context
                .PaymentCurrencies
                .AsNoTracking()
                .Where(x => x.Chain == request.Network)
                .Select(t => _mapper.Map<PaymentCurrencyDTO>(t))
                .ToListAsync(cancellationToken);
        }
    }
}