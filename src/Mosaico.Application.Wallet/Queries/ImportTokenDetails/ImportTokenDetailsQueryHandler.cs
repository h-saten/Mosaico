using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.ValueObjects;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;

namespace Mosaico.Application.Wallet.Queries.ImportTokenDetails
{
    public class ImportTokenDetailsQueryHandler : IRequestHandler<ImportTokenDetailsQuery, ImportTokenDetailsResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public ImportTokenDetailsQueryHandler(IWalletDbContext walletDbContext, IMapper mapper, ITokenService tokenService)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<ImportTokenDetailsResponse> Handle(ImportTokenDetailsQuery request, CancellationToken cancellationToken)
        {
            var contractAddress = new ContractAddress(request.ContractAddress);
            var token = await _walletDbContext
                .Tokens
                .AsNoTracking()
                .Where(t => t.Address == contractAddress.Value)
                .SingleOrDefaultAsync(cancellationToken);

            if (token is not null)
            {
                return new ImportTokenDetailsResponse
                {
                    Decimals = 18,
                    Name = token.Name,
                    Symbol = token.Symbol,
                    CanImport = false
                };
            }

            var tokenDetails = await _tokenService.GetDetailsAsync(request.Chain, request.ContractAddress);
            
            return new ImportTokenDetailsResponse
            {
                Decimals = tokenDetails.Divisor,
                Name = tokenDetails.TokenName,
                Symbol = tokenDetails.Symbol,
                TotalSupply = tokenDetails.TotalSupply,
                CanImport = true,
                Burnable = tokenDetails.Burnable,
                Mintable = tokenDetails.Mintable
            };
        }
    }
}