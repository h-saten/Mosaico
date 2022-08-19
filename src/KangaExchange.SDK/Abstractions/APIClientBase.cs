using System;
using KangaExchange.SDK.Configurations;
using Newtonsoft.Json;
using RestSharp;

namespace KangaExchange.SDK.Abstractions
{
    public abstract class APIClientBase
    {
        protected readonly ISignatureService SignatureGenerateService;
        protected readonly KangaConfiguration KangaConfiguration;
        
        protected APIClientBase(ISignatureService signatureGenerateService, KangaConfiguration kangaConfiguration)
        {
            SignatureGenerateService = signatureGenerateService;
            KangaConfiguration = kangaConfiguration;
        }

        protected string GetBaseUrl()
        {
            return KangaConfiguration.Api.BaseUrl;
        }
        
        protected string GetAppId()
        {
            return KangaConfiguration.Api.AppId;
        }
        
        protected string GetV1Key()
        {
            return KangaConfiguration.Api.V1Key;
        }
        
        protected string GetAppSecretKey()
        {
            return KangaConfiguration.Api.AppSecret;
        }
        
        protected long TimestampRawNonce()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        protected IRestRequest CreateRequest(string uri, object body, bool withSignature = true, string appSecret = null, Method method = Method.POST)
        {
            var requestApi = new RestRequest(uri, DataFormat.Json)
            {
                OnBeforeDeserialization = resp =>
                {
                    resp.ContentType = "application/json";
                },
                Method = method,
                Timeout = TimeSpan.FromSeconds(10).Milliseconds
            };

            if (withSignature)
            {
                appSecret = string.IsNullOrWhiteSpace(appSecret) ? GetAppSecretKey() : appSecret;
                var appSignatureValue = SignatureGenerateService.GenerateSignature(body, appSecret);
                requestApi.AddHeader("api-sig", appSignatureValue);
            }
            
            requestApi.AddJsonBody(body);

            return requestApi;
        }
    }
}