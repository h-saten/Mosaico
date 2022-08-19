using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoinAPI.REST.V1.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Company.CompanyTokenHolders
{
    public class CompanyTokenHoldersQueryHandler : IRequestHandler<CompanyTokenHoldersQuery, CompanyTokenHoldersQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        
        public CompanyTokenHoldersQueryHandler(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<CompanyTokenHoldersQueryResponse> Handle(CompanyTokenHoldersQuery request, CancellationToken cancellationToken)
        {
            var companyTokens = await _walletDbContext
                .Tokens
                .AsNoTracking()
                .Where(x => x.CompanyId == request.CompanyId)
                .ToListAsync(cancellationToken);

            var response = new CompanyTokenHoldersQueryResponse();

            if (companyTokens.Count == 0)
            {
                return response;
            }

            List<Guid> filterByTokenIds;
            if (request.TokenId is not null)
            {
                if (companyTokens.Any(x => x.Id == request.TokenId) is false)
                {
                    throw new UnauthorizedCompanyDataAccess("Token do not belong to DAO");
                }
                filterByTokenIds = new List<Guid> { (Guid) request.TokenId };
            }
            else
            {
                filterByTokenIds = companyTokens.Select(x => x.Id).ToList();
            }

            var holdersQuery = _walletDbContext
                .TokenHolders
                .AsQueryable()
                .Include(x => x.Token)
                .AsNoTracking()
                .OrderByDescending(x => x.Balance)
                .Where(x => filterByTokenIds.Contains(x.TokenId));
                
            var holders = await holdersQuery 
                .Skip(request.Skip)
                .Take(request.Take)
                .Select(x => new CompanyHolderDTO
                {
                    TokenAmount = x.Balance,
                    TokenSymbol = x.Token.Symbol,
                    WalletAddress = x.WalletAddress
                })
                .ToListAsync(cancellationToken);
            response.Entities = holders;
            response.Total = await holdersQuery.CountAsync(cancellationToken);
            
            return response;
        }
    }
}