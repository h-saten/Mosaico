using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using MediatR;
using Mosaico.Application.KangaWallet.DTOs;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.KangaWallet.Queries.UserKangaWalletBalance
{
    public class UserKangaWalletBalanceQueryHandler : IRequestHandler<UserKangaWalletBalanceQuery, KangaWalletBalanceResponse>
    {
        private readonly IKangaUserApiClient _kangaUserApi;
        private readonly ILogger _logger;
        private readonly IUserManagementClient _userManagementClient;
        private readonly IExchangeRateService _exchangeRateService;
        
        public UserKangaWalletBalanceQueryHandler(
            IKangaUserApiClient kangaUserApi, IUserManagementClient userManagementClient, 
            IExchangeRateService exchangeRateService, ILogger logger = null)
        {
            _kangaUserApi = kangaUserApi;
            _userManagementClient = userManagementClient;
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        public async Task<KangaWalletBalanceResponse> Handle(UserKangaWalletBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var kangaUserAccount = await _userManagementClient.GetUserKangaAccountAsync(cancellationToken);
                var profileResponse = await _kangaUserApi.UserProfile(kangaUserAccount.KangaUserId); 
                var profileData = profileResponse.Item2;

                var assets = profileData.Wallet.Where(t => KangaExchange.SDK.Constants.WhiteListedMarkets.Contains(t.Currency)).Select(x => new KangaAssetDto
                {
                    Balance = decimal.Parse(x.Amount, NumberStyles.Float),
                    Symbol = x.Currency,
                    Currency = Wallet.Constants.FIATCurrencies.USD
                })
                .ToList();
                foreach (var asset in assets)
                {
                    var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(asset.Symbol);
                    if (exchangeRate != null)
                    {
                        asset.TotalAssetValue = asset.Balance * exchangeRate.Rate;
                    }
                }
                var currentTotalBalance = assets.Sum(c => c.TotalAssetValue);
                return new KangaWalletBalanceResponse
                {
                    Currency = Wallet.Constants.FIATCurrencies.USD,
                    Assets = assets,
                    TotalWalletValue = currentTotalBalance
                };
            }
            catch (Exception ex)
            {
                _logger.Warning(ex.Message, "Fetching kanga user");
                return new KangaWalletBalanceResponse();
            }
        }
    }
}