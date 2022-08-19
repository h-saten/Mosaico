using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Wallet.Abstractions;
using Mosaico.SDK.Wallet.Models;
using PaymentCurrency = Mosaico.SDK.Wallet.Models.PaymentCurrency;

namespace Mosaico.SDK.Wallet
{
    public class WalletClient : IWalletClient
    {
        private readonly IWalletDbContext _context;
        private readonly IMapper _mapper;

        public WalletClient(IWalletDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> IsInvestorAsync(Guid projectId, string userId)
        {
            return _context.Transactions.AnyAsync(t =>
                t.Status.Key == Constants.TransactionStatuses.Confirmed &&
                t.ProjectId == projectId && t.UserId == userId && t.Type.Key == Constants.TransactionType.Purchase);
        }

        public async Task<TokenWallet> TokenWalletDetails(Guid tokenId, DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null)
        {
            var tokenExist = await _context
                .Tokens
                .Where(m => m.Id == tokenId)
                .AnyAsync();

            if (tokenExist is false)
            {
                throw new TokenNotFoundException(tokenId);
            }
            
            var soldTokensQuery = _context
                .Transactions
                .AsNoTracking()
                .Include(m => m.Status)
                .Include(m => m.Type)
                .Where(t => 
                    t.TokenId == tokenId 
                    && t.Type.Key == Constants.TransactionType.Purchase
                    && t.Status.Key == Constants.TransactionStatuses.Confirmed);

            if (fromDate is not null)
            {
                soldTokensQuery = soldTokensQuery.Where(t => t.LastConfirmationAttemptedAt >= fromDate);
            }     
            if (toDate is not null)
            {
                soldTokensQuery = soldTokensQuery.Where(t => t.LastConfirmationAttemptedAt <= toDate);
            }

            var soldTokens = await soldTokensQuery.SumAsync(m => m.TokenAmount);

            return new TokenWallet
            {
                SoldTokensAmount = soldTokens ?? 0
            };
        }

        public async Task<Tuple<decimal?, decimal?>> GetTokenRaisedAmountAsync(Guid tokenId, Guid? projectId = null)
        {
            var tokenCount = await _context.Tokens.CountAsync(t => t.Id == tokenId);

            if (tokenCount == 0)
            {
                throw new TokenNotFoundException(tokenId);
            }
            
            var soldTokensQuery = _context
                .Transactions
                .AsNoTracking()
                .Include(m => m.Status)
                .Include(m => m.Type)
                .Where(t => 
                    t.TokenId == tokenId && (projectId == null || projectId == t.ProjectId) &&
                    t.PayedAmount != null
                    && t.Type.Key == Constants.TransactionType.Purchase
                    && t.Status.Key == Constants.TransactionStatuses.Confirmed);
            var totalTokenAmount = await soldTokensQuery.SumAsync(m => m.TokenAmount);
            var totalPayedAmount = await soldTokensQuery.SumAsync(m => m.PayedInUSD);
            return new Tuple<decimal?, decimal?>(totalTokenAmount, totalPayedAmount);
        }
        
        public Task<List<Guid>> GetTokensWithExchangeAsync()
        {
            return _context.Tokens.Include(t => t.Exchanges).AsNoTracking().Where(t => t.Exchanges.Any()).Select(t => t.Id).ToListAsync();
        }

        public async Task<List<MosaicoTokenDistribution>> GetTokenDistributionAsync(Guid id)
        {
            var token = await _context.Tokens.Include(t => t.Distributions).AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (token == null)
            {
                throw new TokenNotFoundException(id);
            }

            return token.Distributions.Select(t => new MosaicoTokenDistribution
            {
                Id = t.Id,
                Name = t.Name,
                TokenAmount = t.TokenAmount
            }).ToList();
        }

        public Task<int> GetProjectNFTCountAsync(Guid projectId)
        {
            return _context.NFTs.CountAsync(c => c.NFTCollection.ProjectId == projectId);
        }

        public async Task<int?> GetNumberOfBuyersPerToken(Guid tokenId)
        {
            int? numberOfBuyers = default;
            var token = await _context.Tokens.SingleOrDefaultAsync(m => m.Id == tokenId);
            if (token != null)
            {
                numberOfBuyers = await _context.Transactions.AsNoTracking()
                    .Include(m => m.Status)
                    .Include(m => m.Type)
                    .Where(a => 
                             a.TokenId == token.Id 
                          && a.PayedAmount != null
                          && a.Type.Key == Constants.TransactionType.Purchase
                          && a.Status.Key == Constants.TransactionStatuses.Confirmed)
                    .Select(x => x.UserId)
                    .Distinct()
                    .CountAsync();
            }
            return numberOfBuyers;
        }

        public async Task<TokenWallet> StageTransactionsDetails(Guid tokenId, Guid stageId)
        {
            var tokenCount = await _context.Tokens.CountAsync(t => t.Id == tokenId);

            if (tokenCount == 0)
            {
                throw new TokenNotFoundException(tokenId);
            }
            
            var soldTokensQuery = _context
                .Transactions
                .AsNoTracking()
                .Include(m => m.Status)
                .Include(m => m.Type)
                .Where(t => 
                    t.TokenId == tokenId 
                    && t.StageId == stageId
                    && t.PayedAmount != null
                    && t.Type.Key == Constants.TransactionType.Purchase
                    && t.Status.Key == Constants.TransactionStatuses.Confirmed);

            var soldTokens = await soldTokensQuery.SumAsync(m => m.TokenAmount);
            var raisedUSD = await soldTokensQuery.SumAsync(m => m.PayedInUSD);

            return new TokenWallet
            {
                SoldTokensAmount = soldTokens ?? 0,
                RaisedInUSD = raisedUSD
            };
        }

        public async Task<MosaicoToken> GetTokenAsync(Guid id)
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == id);
            if (token != null)
            {
                //TODO: to automapper
                return _mapper.Map<MosaicoToken>(token);
            }
            return null;
        }

        public async Task<MosaicoToken> GetTokenAsync(string name)
        {
            var token = await _context.Tokens.AsNoTracking().FirstOrDefaultAsync(t => t.Name == name);
            if (token != null)
            {
                //TODO: to automapper
                return _mapper.Map<MosaicoToken>(token);
            }
            return null;
        }

        public async Task<MosaicoCompanyWallet> GetCompanyWalletAsync(Guid companyId, string network)
        {
            var companyWallet = await _context.CompanyWallets.FirstOrDefaultAsync(t => t.CompanyId == companyId && t.Network == network);
            if (companyWallet == null)
            {
                return null;
            }

            var modelCompanyWallet = _mapper.Map<MosaicoCompanyWallet>(companyWallet);
            modelCompanyWallet.Tokens = companyWallet.Tokens.Select(t => _mapper.Map<MosaicoToken>(t.Token)).ToList();
            return modelCompanyWallet;
        }
        
        public async Task<MosaicoUserWallet> GetUserWalletAsync(string userId, string network)
        {
            var userWallet = await _context.Wallets.FirstOrDefaultAsync(t => t.UserId == userId && t.Network == network);
            if (userWallet == null)
            {
                return null;
            }
            var modelUserWallet = _mapper.Map<MosaicoUserWallet>(userWallet);
            modelUserWallet.Tokens = userWallet.Tokens.Select(t => _mapper.Map<MosaicoToken>(t.Token)).ToList();
            return modelUserWallet;
        }
            
        public async Task MintTokensToCompanyWallet(Guid companyId, Guid tokenId)
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == tokenId);
            if (token == null)
            {
                throw new TokenNotFoundException(tokenId);
            }

            var companyWallet =
                await _context.CompanyWallets.FirstOrDefaultAsync(c =>
                    c.CompanyId == companyId && c.Network == token.Network);
            if (companyWallet == null)
            {
                throw new CompanyWalletNotFoundException(companyId.ToString());
            }

            if (!companyWallet.Tokens.Any(t => t.TokenId == tokenId))
            {
                companyWallet.Tokens.Add(new CompanyWalletToToken
                {
                    Token = token,
                    CompanyWallet = companyWallet,
                    TokenId = tokenId,
                    CompanyWalletId = companyWallet.Id
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetTokenDeployedAsync(Guid id, string ownerAddress, string contractAddress, string version)
        {
            //TODO: validate
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == id);
            token.OwnerAddress = ownerAddress;
            token.ContractVersion = version;
            token.Address = contractAddress;
            token.Status = TokenStatus.Deployed;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTokenAsync(MosaicoToken tokenDTO)
        {
            //TODO: validate
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == tokenDTO.Id);
            if (token == null)
            {
                throw new TokenNotFoundException(tokenDTO.Id);
            }

            var type = await _context.TokenTypes.FirstOrDefaultAsync(t => t.Key == tokenDTO.Type);
            if (type == null)
            {
                throw new TokenTypeNotFoundException(tokenDTO.Type);
            }

            token.Name = tokenDTO.Name;
            token.Network = tokenDTO.Network;
            token.Symbol = tokenDTO.Symbol;
            token.TotalSupply = tokenDTO.TotalSupply;
            token.IsBurnable = tokenDTO.IsBurnable;
            token.IsMintable = tokenDTO.IsMintable;
            token.SetType(type);
            
            await _context.SaveChangesAsync();
        }

        public async Task<MosaicoToken> CreateTokenAsync(string name, string symbol, long totalSupply, string network, bool isMintable, bool isBurnable, string type)
        {
            //TODO: validate
            var tokenType = await _context.TokenTypes.FirstOrDefaultAsync(t => t.Key == type);
            if (tokenType == null)
            {
                throw new TokenNotFoundException(type);
            }
            var token = new Token
            {
                Name = name,
                Network = network,
                Symbol = symbol,
                TotalSupply = totalSupply,
                TokensLeft = totalSupply,
                IsBurnable = isBurnable,
                IsMintable = isMintable
            };
            token.Status = TokenStatus.Pending;
            token.SetType(tokenType);
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
            return _mapper.Map<MosaicoToken>(token);
        }

        public async Task<InvestorTokenSummary> GetTokenSummaryAsync(Guid tokenId, Guid userId)
        {
            var tokenSymbol = await _context
                .Tokens
                .AsNoTracking()
                .Where(x => x.Id == tokenId)
                .Select(x => x.Symbol)
                .SingleOrDefaultAsync();

            var purchaseTypeId = await _context
                .TransactionType
                .AsNoTracking()
                .Where(t => t.Key == Constants.TransactionType.Purchase)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();
            
            var confirmedStatusId = await _context
                .TransactionStatuses
                .AsNoTracking()
                .Where(t => t.Key == Constants.TransactionStatuses.Confirmed)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();
            
            var userTokensAmount = await _context
                .Transactions
                .AsNoTracking()
                .Where(x => x.UserId == userId.ToString() 
                            && x.TokenId == tokenId 
                            && x.StatusId == confirmedStatusId
                            && x.TypeId == purchaseTypeId)
                .SumAsync(x => x.TokenAmount);
            
            var userFinalizedTransactionsDates = await _context
                .Transactions
                .AsNoTracking()
                .Where(x => x.UserId == userId.ToString() 
                            && x.TokenId == tokenId 
                            && x.TypeId == purchaseTypeId
                            && x.StatusId == confirmedStatusId)
                .OrderBy(x => x.FinishedAt)
                .Select(x => x.FinishedAt)
                .ToListAsync();
            var userFinalizedTransactionsAmount = userFinalizedTransactionsDates.Count;

            var userSequenceNumber = 0;
            var firstUserTransactionDate = userFinalizedTransactionsDates.First();

            if (firstUserTransactionDate is not null)
            {
                var investorsBeforeFirstUserTransaction = await _context
                    .Transactions
                    .AsNoTracking()
                    .Where(x => x.UserId != userId.ToString() 
                                && x.TokenId == tokenId 
                                && x.TypeId == purchaseTypeId 
                                && x.StatusId == confirmedStatusId
                                && x.FinishedAt < firstUserTransactionDate)
                    .CountAsync();
                userSequenceNumber = investorsBeforeFirstUserTransaction + 1;
            }

            var response = new InvestorTokenSummary
            {
                TokenSymbol = tokenSymbol,
                PaidTokensAmount = userTokensAmount ?? 0,
                InvestorFinalizedTransactions = userFinalizedTransactionsAmount,
                LastTransactionDate = userFinalizedTransactionsDates.Last(),
                InvestorSequenceNumber = userSequenceNumber,
            };

            return response;
        }        
        
        public async Task<string> GetPaymentCurrencyAddressAsync(string tokenSymbol, string network)
        {
            var paymentCurrency = await _context.PaymentCurrencies.FirstOrDefaultAsync(pc => pc.Ticker == tokenSymbol);
            return paymentCurrency == null ? 
                string.Empty : paymentCurrency.ContractAddress;
        }
        
        public async Task<List<string>> GetPaymentCurrencyAddressesAsync(string chain)
        {
            var contractAddresses = await _context
                .PaymentCurrencies
                .AsNoTracking()
                .Where(x => x.Chain == chain)
                .Select(x => x.ContractAddress)
                .ToListAsync();

            return contractAddresses;
        }

        public async Task<List<PaymentCurrency>> GetPaymentCurrenciesAsync(string chain)
        {
            var currencies = await _context
                .PaymentCurrencies
                .AsNoTracking()
                .Where(x => x.Chain == chain)
                .Select(x => new PaymentCurrency
                {
                    Name = x.Name,
                    Symbol = x.Ticker,
                    ContractAddress = x.ContractAddress,
                    IsNativeCurrency = x.NativeChainCurrency
                })
                .ToListAsync();

            return currencies;
        }

        public async Task<TransactionDetails> GetTransactionAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
            return transaction == null ? null : _mapper.Map<TransactionDetails>(transaction);
        }

        public async Task<decimal> ExchangeRateAsync(string ticker)
        {
            var currencyTicker = ticker.ToUpperInvariant();
            var exchangeRate = await _context.ExchangeRates.FirstOrDefaultAsync(t => t.Ticker == currencyTicker);
            return exchangeRate?.Rate ?? 0;
        }
    }
}