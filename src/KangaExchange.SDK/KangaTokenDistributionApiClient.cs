using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using KangaExchange.SDK.Exceptions;
using KangaExchange.SDK.Models;
using KangaExchange.SDK.Models.TokenDistribution;
using RestSharp;
using Serilog;

namespace KangaExchange.SDK
{
    public class KangaTokenDistributionApiClient : APIClientBase, IKangaTokenDistributionApiClient
    {
        private readonly ILogger _logger;
        public KangaTokenDistributionApiClient(ISignatureService signatureGenerateService,
            KangaConfiguration kangaConfiguration, ILogger logger = null) : base(signatureGenerateService, kangaConfiguration)
        {
            _logger = logger;
        }

        public async Task<StandardResponse> Transfer(string from, string to, string currency, decimal quantity)
        {
            var apiRequestUrl = "rest/wallet/transfer";

            quantity = Math.Round(quantity, 8);

            var body = new
            {
                from = string.IsNullOrEmpty(from) ? null : from,
                to,
                currency,
                quantity = quantity.ToString(CultureInfo.InvariantCulture),
                apiKey = GetV1Key()
            };

            var requestApi = CreateRequest(apiRequestUrl, body, false);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<StandardResponse>(requestApi);

            if (!responseApi.IsSuccessful || responseApi.Data.Result != "ok")
            {
                _logger?.Error("Error while try made transfer by Kanga api. " + responseApi.ErrorMessage);
                throw new KangaException("kanga_api_connect_error");
            }

            responseApi.Data.SuccessResult = responseApi.Data.Result == "ok";

            return responseApi.Data;
        }

        public async Task<WalletResponse> WalletBalance(string walletName = null)
        {
            var apiRequestUrl = "rest/wallet";

            var body = new
            {
                walletKey = string.IsNullOrEmpty(walletName) ? null : walletName,
                apiKey = GetV1Key()
            };

            var requestApi = CreateRequest(apiRequestUrl, body, false);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<WalletResponse>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error("Error while try get wallet balance from kanga api. " + responseApi.ErrorMessage);
                throw new KangaException("kanga_api_connect_error");
            }

            return responseApi.Data;
        }

        public async Task<StandardResponse> WalletShift(string fromWalletName, string toWalletName, string currency,
            decimal quantity)
        {
            var apiRequestUrl = "rest/wallet/shift";

            quantity = Math.Round(quantity, 8);

            var body = new WalletShiftRequest
            {
                from = string.IsNullOrEmpty(fromWalletName) ? string.Empty : fromWalletName,
                to = string.IsNullOrEmpty(toWalletName) ? string.Empty : toWalletName,
                currency = currency,
                quantity = quantity.ToString(CultureInfo.InvariantCulture),
                apiKey = GetV1Key()
            };

            var requestApi = CreateRequest(apiRequestUrl, body, false);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<StandardResponse>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error(
                    "Error while try shift from wallet to wallet by kanga api. " + responseApi.ErrorMessage);
                throw new KangaException("kanga_api_connect_error");
            }

            responseApi.Data.SuccessResult = responseApi.Data.Result == "ok";

            return responseApi.Data;
        }

        public async Task<StandardResponse> CreateWallet(string name)
        {
            var apiRequestUrl = "rest/wallet/create";

            var body = new
            {
                walletKey = name,
                apiKey = GetV1Key()
            };

            var requestApi = CreateRequest(apiRequestUrl, body, false);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<StandardResponse>(requestApi);

            if (!responseApi.IsSuccessful || responseApi.Data.Result != "ok")
            {
                _logger?.Error($"Error while try create kanga wallet {name} by api. " + responseApi.ErrorMessage);
                throw new KangaException("kanga_api_connect_error");
            }

            responseApi.Data.SuccessResult = responseApi.Data.Result == "ok";

            return responseApi.Data;
        }

        public async Task<decimal> WalletBalanceForToken(string tokenTicker, string walletName = null)
        {
            var walletResponse = await WalletBalance(walletName);

            var walletBalanceAsString = walletResponse
                .Wallets
                .Where(m => m.CurrencyCode == tokenTicker.ToString())
                .Select(m => m.Value)
                .SingleOrDefault();

            if (walletBalanceAsString == null) return decimal.Zero;

            return decimal.Parse(walletBalanceAsString, NumberStyles.Float);
        }
    }
}