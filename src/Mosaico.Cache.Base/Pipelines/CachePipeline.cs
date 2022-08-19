using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Cache.Base.Attributes;
using System.Reflection;
using Mosaico.Base.Abstractions;
using Mosaico.Cache.Base.Abstractions;

namespace Mosaico.Cache.Base.Pipelines
{
    public class CachePipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ICacheClient _cacheClient;
        private readonly ITemplateEngine _templateEngine;

        public CachePipeline(ITemplateEngine templateEngine, ICacheClient cacheClient = null)
        {
            _templateEngine = templateEngine;
            _cacheClient = cacheClient;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_cacheClient == null || !_cacheClient.IsEnabled)
            {
                return await next();
            }

            var attribute = request.GetType().GetCustomAttribute<CacheAttribute>();
            if (attribute != null)
            {
                var key = $"{typeof(TRequest).Name}";
                if (!string.IsNullOrWhiteSpace(attribute.Pattern))
                {
                    key += $"_{_templateEngine.Build(attribute.Pattern, request)}";
                }
                
                if (!string.IsNullOrWhiteSpace(key))
                {
                    var redisPayload = await _cacheClient?.GetAsync<TResponse>(key, cancellationToken);
                    if (redisPayload != null)
                    {
                        return redisPayload;
                    }

                    var response = await next();
                    await _cacheClient?.AddAsync(key, response, attribute.ExpirationInMinutes, token: cancellationToken);
                    return response;
                }
            }
            
            return await next();
        }
    }
}