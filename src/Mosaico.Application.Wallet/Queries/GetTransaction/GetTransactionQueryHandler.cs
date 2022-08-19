using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.GetTransaction
{
    public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, TransactionDTO>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;

        public GetTransactionQueryHandler(IWalletDbContext walletDbContext, IMapper mapper)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
        }

        public async Task<TransactionDTO> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (transaction == null)
            {
                throw new TransactionNotFoundException(request.Id.ToString());
            }
            
            var dto = _mapper.Map<TransactionDTO>(transaction);
            if (transaction.TokenId.HasValue)
            {
                var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == transaction.TokenId.Value, cancellationToken);
                if (token != null)
                {
                    dto.Token = _mapper.Map<TokenDTO>(token);
                }
            }
            else if (transaction.PaymentCurrencyId.HasValue)
            {
                var contract = transaction.PaymentCurrency;
                if (contract != null)
                {
                    dto.Token = new TokenDTO
                    {
                        Address = contract.ContractAddress,
                        Network = contract.Chain,
                        Name = transaction.PaymentCurrency.Name,
                        Symbol = transaction.PaymentCurrency.Ticker,
                        LogoUrl = transaction.PaymentCurrency.LogoUrl
                    };
                }
            }

            return dto;
        }
    }
}