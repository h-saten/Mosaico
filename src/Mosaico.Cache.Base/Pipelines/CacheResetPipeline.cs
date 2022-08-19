using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Reflection;
using Mosaico.Base.Abstractions;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Cache.Base.Attributes;
using Serilog;

namespace Mosaico.Cache.Base.Pipelines
{
    public class CacheResetPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly ITemplateEngine _templateEngine;
        private readonly ICacheClient _cacheClient;
        public CacheResetPipeline(ITemplateEngine templateEngine, ICacheClient cacheClient = null, ILogger logger = null)
        {
            _templateEngine = templateEngine;
            _cacheClient = cacheClient;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_cacheClient == null || !_cacheClient.IsEnabled)
            {
                return await next();
            }
            
            var result = await next();
            
            try
            {
                var attributes = request.GetType().GetCustomAttributes<CacheResetAttribute>();
                if (attributes != null)
                {
                    foreach (var attribute in attributes)
                    {
                        if (!string.IsNullOrWhiteSpace(attribute.Pattern))
                        {
                            var key = $"{attribute.RequestName}_{_templateEngine.Build(attribute.Pattern, request)}";
                            await _cacheClient.CleanAsync(key, cancellationToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.Error($"Failed during cache cleanup with error: {ex.Message} / {ex.StackTrace}");
            }

            return result;
        }
    }
}