using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Mosaico.CQRS.Base.Pipelines
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger logger = null)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _logger?.Information($"Request {typeof(TRequest).Name} started");
            var response = await next();
            _logger?.Information($"Request {typeof(TResponse).Name} was successfully handled");
            return response;
        }
    }
}