using System;
using System.Globalization;
using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using KangaExchange.SDK.Exceptions;
using KangaExchange.SDK.Models;
using KangaExchange.SDK.Models.BuyClient;
using RestSharp;
using Serilog;

namespace KangaExchange.SDK
{
    public class KangaBuyApiClient : APIClientBase, IKangaBuyApiClient
    {
        private readonly ILogger _logger;
        private readonly IKangaEstimateProcessor _estimateProcessor;
        
        public KangaBuyApiClient(ISignatureService signatureGenerateService, KangaConfiguration kangaConfiguration, IKangaEstimateProcessor estimateProcessor, ILogger logger = null) :
            base(signatureGenerateService, kangaConfiguration)
        {
            _estimateProcessor = estimateProcessor;
            _logger = logger;
        }

        public virtual async Task<EstimatesResponseDto> GetEstimatesAsync(string tokenTicker)
        {
            var apiRequestUrl = "wallet/currency/converter/estimates";

            var body = new
            {
                toCurrency = tokenTicker
            };

            var requestApi = CreateRequest(apiRequestUrl, body, false);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<EstimatesApiResponseDto>(requestApi);

            if (!responseApi.IsSuccessful || responseApi.Data.Result != "ok")
            {
                _logger?.Error("Error while try get estimates from kanga api. " + responseApi.ErrorMessage);
                throw new KangaException("kanga_api_connect_error");
            }

            var processedEstimates = _estimateProcessor.Process(responseApi.Data);

            return processedEstimates;
        }

        public async Task<BuyResponseDto> BuyAsync(Action<BuyParams> configuration)
        {
            var apiRequestUrl = "wallet/currency/convert";

            var parameters = new BuyParams();
            configuration.Invoke(parameters);

            var requestData = new BuyRequestData
            {
                email = parameters.Email,
                quantity = parameters.CurrencyAmount.ToString(CultureInfo.InvariantCulture),
                buyCode = parameters.BuyCode,
                fromCurrency = parameters.PaymentCurrency.ToString(),
                paymentType = parameters.PaymentCurrency.SupportedTransactionType(),
                toCurrency = parameters.TokenSymbol.ToUpper(), // MOSA2 ieo is available on kanga.dev environment
                customRedirectUrl = parameters.CustomRedirectUrl
            };

            var requestApi = CreateRequest(apiRequestUrl, requestData, false);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<BuyResponseDto>(requestApi);

            if (!responseApi.IsSuccessful || responseApi.Data.Result != "ok")
            {
                _logger?.Error("Error while try buy by kanga api. " + responseApi.ErrorMessage);
                throw new KangaException("kanga_api_connect_error");
            }

            _logger?.Information("Successfully requested buy in Kanga.");

            var tpgPaymentId = PaymentGatewayIdFromUrlException.Extract(responseApi.Data.RedirectUrl);
            responseApi.Data.OrderId = tpgPaymentId;
            
            return responseApi.Data;
        }

        public async Task<TransactionResponseDto> GetTransaction(string transactionIdValue)
        {
            var apiRequestUrl = "v2/ieo/transaction";

            var body = new
            {
                appId = GetAppId(),
                nonce = TimestampRawNonce(),
                transactionId = transactionIdValue,
                notificationUrl = KangaConfiguration.Api.TransactionCallback
            };

            var requestApi = CreateRequest(apiRequestUrl, body);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<TransactionResponseDto>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error($"Error while try get transaction '{transactionIdValue}' from kanga api.");
                throw new KangaException("kanga_api_connect_error");
            }

            if (responseApi.Data.Result != "ok")
            {
                var errorCode = responseApi.Data.Code;

                if (errorCode == 9000) _logger?.Error($"[{nameof(GetTransaction)}] invalid signature");
                if (errorCode == 9001) _logger?.Error($"[{nameof(GetTransaction)}] access denied");
                if (errorCode == 9002) _logger?.Error($"[{nameof(GetTransaction)}] invalid parameters");

                throw new KangaException(errorCode, "Request error while getting transaction from Kanga");
            }

            _logger?.Information("Successfully request getting transaction from Kanga.");

            return responseApi.Data;
        }
    }
}