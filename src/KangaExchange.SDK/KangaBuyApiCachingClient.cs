using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace KangaExchange.SDK
{
    // public class KangaBuyApiCachingClient : KangaBuyApiClient
    // {
    //     private readonly ILogger<KangaBuyApiCachingClient> _logger;
    //
    //     public KangaBuyApiCachingClient(
    //         ILogger<KangaBuyApiCachingClient> logger,
    //         IOptions<KangaApiConfiguration> kangaApiConfig, 
    //         IOptions<AppInfo> appInfoConfig,
    //         ICacheFiles cacheFiles)
    //         : base(logger, kangaApiConfig, appInfoConfig)
    //     {
    //         _restClient = new RestClient(GetBaseUrl());
    //         _logger = logger;
    //         _cache = cacheFiles;
    //         _appInfoConfig = appInfoConfig.Value;
    //     }
    //
    //     public override async Task<EstimatesResponseDto> GetEstimatesAsync(string tokenTicker)
    //     {
    //        var cacheFileName = nameof(EstimatesResponseDto) + $"_{tokenTicker}";
    //
    //         if (_cache.CacheExist(cacheFileName))
    //         {
    //             return _cache.GetFileFromCache<EstimatesResponseDto>(cacheFileName);
    //         }
    //         
    //         var response = await base.GetEstimatesAsync(tokenTicker);
    //         await _cache.OverrideCacheFileAsync(cacheFileName, response, DateTime.UtcNow.AddSeconds(60));
    //         return response;
    //     }
    // }
}