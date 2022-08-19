using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using KangaExchange.SDK.Exceptions;
using KangaExchange.SDK.Models;
using RestSharp;
using Serilog;

namespace KangaExchange.SDK
{
    public class KangaAuthClient : APIClientBase, IKangaAuthAPIClient
    {
        private readonly ILogger _logger;

        public KangaAuthClient(ISignatureService signatureGenerateService, KangaConfiguration kangaConfiguration, ILogger logger = null) : base(signatureGenerateService, kangaConfiguration)
        {
            _logger = logger;
        }

        public async Task<IRestResponse<CheckResponseDTO>> CheckAsync(string token)
        {
            var apiRequestUrl = Constants.KangaAPIRoutes.Check;
            var body = new 
            {
                appId = GetAppId(),
                token
            };
            var requestApi = CreateRequest(apiRequestUrl, body);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<CheckResponseDTO>(requestApi);
            
            if (!responseApi.IsSuccessful)
            {
                _logger?.Error($"[{nameof(KangaAuthClient)}] Error while make check request for token: '{token}'. Error: {responseApi.Content}");
                throw new KangaException("0000", $"kanga_api_connect_error: {responseApi.Content}");
            }
            
            if (responseApi.Data.Result != Constants.KangaResponseStatuses.OK)
            {
                var errorCode = responseApi.Data.Code;
                if (!Constants.KangaErrorCodes.ContainsKey(errorCode))
                {
                    throw new KangaException("0000", "Unknown Kanga error occured.");
                }

                throw new KangaException(errorCode, Constants.KangaErrorCodes[errorCode]);
            }
            _logger?.Verbose($"Successfully request 'check' to Kanga oAuth endpoint.");
            return responseApi;
        }

        public async Task<IRestResponse<ProfileResponseDTO>> ProfileAsync(string userId)
        {
            var apiRequestUrl = Constants.KangaAPIRoutes.Profile;
            
            var body = new 
            {
                appId = GetAppId(),
                appUserId = userId
            };

            var requestApi = CreateRequest(apiRequestUrl, body);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<ProfileResponseDTO>(requestApi);
            
            if (!responseApi.IsSuccessful)
            {
                _logger?.Error($"[{nameof(KangaAuthClient)}] Error while make check request for token: '{userId}'. Error: {responseApi.Content}");
                throw new KangaException("0000", $"kanga_api_connect_error: {responseApi.Content}");
            }
            
            if (responseApi.Data.Result != Constants.KangaResponseStatuses.OK)
            {
                var errorCode = responseApi.Data.Code;
                if (!Constants.KangaErrorCodes.ContainsKey(errorCode))
                {
                    throw new KangaException("0000", "Unknown Kanga error occured.");
                }

                throw new KangaException(errorCode, Constants.KangaErrorCodes[errorCode]);
            }
            _logger?.Verbose($"Successfully request 'check' to Kanga oAuth endpoint.");
            return responseApi;
        }
    }
}