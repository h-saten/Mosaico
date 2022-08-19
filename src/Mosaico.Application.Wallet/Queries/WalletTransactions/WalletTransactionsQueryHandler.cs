using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.WalletTransactions
{
    public class WalletTransactionsQueryHandler : IRequestHandler<WalletTransactionsQuery, WalletTransactionsResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;

        public WalletTransactionsQueryHandler(IWalletDbContext walletDbContext, IMapper mapper)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
        }

        public async Task<WalletTransactionsResponse> Handle(WalletTransactionsQuery request, CancellationToken cancellationToken)
        {
            var userWallet = await _walletDbContext
                .Wallets
                .FirstOrDefaultAsync(u => u.AccountAddress == request.WalletAddress && u.Network == request.Network, cancellationToken);
            
            if (userWallet == null)
                throw new WalletNotFoundException(request.WalletAddress, request.Network);

            var transactions = _walletDbContext
                .Transactions
                .Include(m => m.Status)
                .Include(m => m.Type)
                .Include(m => m.PaymentCurrency)
                .AsNoTracking()
                .Where(m => (m.From == request.WalletAddress || m.To == request.WalletAddress) && m.FinishedAt != null 
                        && m.Network == request.Network && m.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed)
                .OrderByDescending(t => t.FinishedAt);
            
            var items = await transactions.Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync(cancellationToken);
            var count = await transactions.CountAsync(cancellationToken: cancellationToken);
            
            var transactionTokenIds = items.Select(m => m.TokenId).Distinct().ToList();

            var tokens = await _walletDbContext
                .Tokens
                .Include(t => t.Type)
                .Include(t => t.Exchanges).ThenInclude(e => e.ExternalExchange)
                .Where(m => transactionTokenIds.Contains(m.Id))
                .ToListAsync(cancellationToken);

            var dtos = new List<TransactionDTO>();
            
            foreach (var transaction in items)
            {
                var dto = _mapper.Map<TransactionDTO>(transaction);
                dto.TransactionDirection = string.Equals(dto.To, request.WalletAddress, StringComparison.InvariantCultureIgnoreCase)
                    ? TransactionDirection.INCOMING
                    : TransactionDirection.OUTGOING;
                if (transaction.TokenId.HasValue)
                {
                    var token = tokens.FirstOrDefault(t => t.Id == transaction.TokenId.Value);
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
                dtos.Add(dto);
            }

            return new WalletTransactionsResponse
            {
                Entities = dtos,
                Total = count
            };
        }
    }
}