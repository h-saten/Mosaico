using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using KangaExchange.SDK.Exceptions;
using KangaExchange.SDK.Models;
using RestSharp;
using Serilog;

namespace KangaExchange.SDK
{
    public class KangaIssuerApiClient : APIClientBase, IKangaIssuerApiClient
    {
        private readonly ILogger _logger;
        
        public KangaIssuerApiClient(ISignatureService signatureGenerateService, KangaConfiguration kangaConfiguration, ILogger logger = null) :
            base(signatureGenerateService, kangaConfiguration)
        {
            _logger = logger;
        }

        public async Task<IRestResponse<ReportResponseDto>> ReportAsync(string tokenTicker)
        {
            var apiRequestUrl = "v2/ieo/report";

            var timestamp = TimestampRawNonce();

            var body = new ReportBodyDTO
            {
                AppId = GetAppId(),
                Nonce = timestamp,
                Code = tokenTicker
            };

            var requestApi = CreateRequest(apiRequestUrl, body);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<ReportResponseDto>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error($"[{nameof(KangaIssuerApiClient)}] Error while get report for '{tokenTicker}'.");
                throw new KangaException("0000", "kanga_api_connect_error");
            }

            if (responseApi.Data.Result != "ok")
            {
                var errorCode = responseApi.Data.Code;

                if (errorCode == "9000") _logger?.Error($"[{nameof(KangaIssuerApiClient)}] invalid signature");
                if (errorCode == "9001") _logger?.Error($"[{nameof(KangaIssuerApiClient)}] access denied");
                if (errorCode == "9002")
                    _logger?.Error($"[{nameof(KangaIssuerApiClient)}] invalid parameters - missing ieo");

                throw new KangaException(errorCode, "Request error while get report from Kanga");
            }

            _logger?.Information("Successfully request report in Kanga.");

            return responseApi;
        }
    }
}