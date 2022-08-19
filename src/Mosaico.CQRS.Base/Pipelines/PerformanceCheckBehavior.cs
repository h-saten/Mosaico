using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Mosaico.CQRS.Base.Pipelines
{
    public class PerformanceCheckBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public PerformanceCheckBehavior(ILogger logger = null)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                var response = await next();
                sw.Stop();
                return response;
            }
            catch (Exception)
            {
                sw.Stop();
                throw;
            }
            finally
            {
                if (sw.ElapsedMilliseconds > 1000)
                    _logger?.Warning($"Request {typeof(TRequest).Name} took {sw.Elapsed} to execute.");
            }
        }
    }
}