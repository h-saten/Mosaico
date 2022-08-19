using System;
using System.Globalization;
using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using KangaExchange.SDK.Exceptions;
using KangaExchange.SDK.Models;
using RestSharp;
using Serilog;

namespace KangaExchange.SDK
{
    public class KangaAffiliationApiClient : APIClientBase, IKangaAffiliationApiClient
    {
        private readonly ILogger _logger;
        
        public KangaAffiliationApiClient(ISignatureService signatureGenerateService,
            KangaConfiguration kangaConfiguration, ILogger logger = null) : base(signatureGenerateService, kangaConfiguration)
        {
            _logger = logger;
        }

        public async Task<IRestResponse<SavePartnerResponseDto>> SavePartnerAsync(ISavePartnerParameters parameters)
        {
            var apiRequestUrl = "v2/ieo/partner/save";

            var nonce = TimestampRawNonce();

            var body = new SavePartnerBody
            {
                appId = parameters.KangaApiPublicKey ?? GetAppId(),
                nonce = nonce,
                ieoCode = parameters.IcoTicker,
                email = parameters.Email.ToLower(),
                buyCode = parameters.Reflink,
                feeRate = (parameters.CurrencyFeeRate / 100).ToString(CultureInfo.InvariantCulture),
                bonusRate = (parameters.TokensFeeRate / 100).ToString(CultureInfo.InvariantCulture)
            };
            if (parameters.FromDate != null) body.fromDate = parameters.FromDate.ToString();
            if (parameters.ToDate != null) body.toDate = parameters.ToDate.ToString();

            Console.WriteLine(apiRequestUrl);
            Console.WriteLine(parameters.KangaApiPublicKey);
            Console.WriteLine(parameters.KangaApiPrivateKey);

            var requestApi = CreateRequest(apiRequestUrl, body, true, parameters.KangaApiPrivateKey);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<SavePartnerResponseDto>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error($"Error while try save partner '{parameters.Email}' in kanga api.");
                throw new KangaException("0000", "kanga_api_connect_error");
            }

            if (responseApi.Data.Result != "ok")
            {
                var errorCode = responseApi.Data.Code;

                if (errorCode == "9000") _logger?.Error($"[{nameof(SavePartnerAsync)}] invalid signature");
                if (errorCode == "9001") _logger?.Error($"[{nameof(SavePartnerAsync)}] access denied");
                if (errorCode == "9002") _logger?.Error($"[{nameof(SavePartnerAsync)}] invalid parameters");
                if (errorCode == "9003")
                {
                    _logger?.Error($"[{nameof(SavePartnerAsync)}] partner has no account in Kanga");
                    throw new KangaException(errorCode, "email_not_found_in_kanga");
                }

                throw new KangaException("0000", "Request error while saving partner in Kanga");
            }

            _logger?.Information("Successfully request saving partner in Kanga.");

            return responseApi;
        }

        public async Task<IRestResponse<PartnerCodesResponseDto>> PartnerCodesAsync(string partnerEmailAddress)
        {
            var apiRequestUrl = "v2/ieo/partner/codes";

            var body = new
            {
                appId = GetAppId(),
                nonce = TimestampRawNonce(),
                partnerEmail = partnerEmailAddress
            };

            var requestApi = CreateRequest(apiRequestUrl, body);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<PartnerCodesResponseDto>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error($"Error while try get partner codes '{partnerEmailAddress}' in kanga api.");
                throw new KangaException("kanga_api_connect_error");
            }

            if (responseApi.Data.Result != "ok")
            {
                var errorCode = responseApi.Data.Code;

                if (errorCode == "9000") _logger?.Error($"[{nameof(PartnerCodesAsync)}] invalid signature");
                if (errorCode == "9001") _logger?.Error($"[{nameof(PartnerCodesAsync)}] access denied");
                if (errorCode == "9002") _logger?.Error($"[{nameof(PartnerCodesAsync)}] invalid parameters");

                throw new KangaException("Request error while get partner codes from Kanga");
            }

            _logger?.Information("Successfully request saving partner in Kanga.");

            responseApi = ProcessFeeFromDecimalToPercentage(responseApi);

            return responseApi;
        }

        public async Task<IRestResponse<PartnerReportResponseDto>> PartnerReportAsync(string partnerEmailAddress)
        {
            var apiRequestUrl = "v2/ieo/partner/report";

            var body = new
            {
                appId = GetAppId(),
                nonce = TimestampRawNonce(),
                partnerEmail = partnerEmailAddress
            };

            var requestApi = CreateRequest(apiRequestUrl, body);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<PartnerReportResponseDto>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error($"Error while try get partner '{partnerEmailAddress}' report from kanga api.");
                throw new KangaException("kanga_api_connect_error");
            }

            if (responseApi.Data.Result != "ok")
            {
                var errorCode = responseApi.Data.Code;

                if (errorCode == "9000") _logger?.Error($"[{nameof(PartnerCodesAsync)}] invalid signature");
                if (errorCode == "9001")
                    _logger?.Error($"[{nameof(PartnerCodesAsync)}] access denied - no permission");
                if (errorCode == "9002")
                    _logger?.Error($"[{nameof(PartnerCodesAsync)}] '{partnerEmailAddress}' no partner");

                throw new KangaException("Request error while get partner codes from Kanga");
            }

            _logger?.Information("Successfully request saving partner in Kanga.");

            return responseApi;
        }

        private IRestResponse<PartnerCodesResponseDto> ProcessFeeFromDecimalToPercentage(
            IRestResponse<PartnerCodesResponseDto> response)
        {
            var data = response.Data;

            foreach (var code in data.Codes)
            {
                var feeAsDecimal = code.FeeRate;
                var feeAsPercentage = Convert.ToDouble(feeAsDecimal, CultureInfo.InvariantCulture) * 100;
                code.FeeRate = feeAsPercentage.ToString(CultureInfo.InvariantCulture);
            }

            return response;
        }
    }
}