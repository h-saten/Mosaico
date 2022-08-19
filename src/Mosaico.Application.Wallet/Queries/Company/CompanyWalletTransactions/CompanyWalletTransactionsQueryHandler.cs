using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Company.CompanyWalletTransactions
{
    public class CompanyWalletTransactionsQueryHandler : IRequestHandler<CompanyWalletTransactionsQuery, CompanyWalletTransactionsResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;

        public CompanyWalletTransactionsQueryHandler(IWalletDbContext walletDbContext, IMapper mapper)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
        }

        public async Task<CompanyWalletTransactionsResponse> Handle(CompanyWalletTransactionsQuery request, CancellationToken cancellationToken)
        {
            var companyWallet = await _walletDbContext
                    .CompanyWallets
                    .FirstOrDefaultAsync(u => u.CompanyId == request.CompanyId, cancellationToken);
            
            if (companyWallet == null) 
                throw new CompanyWalletNotFoundException(request.CompanyId.ToString());
            
            var query = _walletDbContext
                .Transactions
                .Include(m => m.Status)
                .Include(m => m.Type)
                .Include(m => m.PaymentCurrency)
                .AsNoTracking()
                .Where(m => (m.From == companyWallet.AccountAddress || m.To == companyWallet.AccountAddress) && m.FinishedAt != null && m.Network == companyWallet.Network && m.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed)
                .OrderByDescending(t => t.FinishedAt);
            
            var items = await query.Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync(cancellationToken);
            var count = await query.CountAsync(cancellationToken: cancellationToken);
            
            var transactionTokenIds = items.Select(m => m.TokenId).Distinct().ToList();

            var tokens = await _walletDbContext
                .Tokens
                .Include(t => t.Type)
                .Include(t => t.Exchanges).ThenInclude(e => e.ExternalExchange)
                .Where(m => transactionTokenIds.Contains(m.Id))
                .ToListAsync(cancellationToken);
            
            var dtos = new List<CompanyTransactionDTO>();
            
            //TODO: make use of redis
            
            foreach (var transaction in items)
            {
                var dto = _mapper.Map<CompanyTransactionDTO>(transaction);
                dto.TransactionDirection = string.Equals(dto.To, companyWallet.AccountAddress, StringComparison.InvariantCultureIgnoreCase)
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
            
            return new CompanyWalletTransactionsResponse
            {
                Entities = dtos,
                Total = count
            };
        }
    }
}