using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Features.Abstractions;
using Mosaico.Domain.Features.Exceptions;
using Serilog;

namespace Mosaico.Application.Features.Commands.UpdateBetaTester
{
    public class UpdateBetaTesterCommandHandler : IRequestHandler<UpdateBetaTesterCommand>
    {
        private readonly ILogger _logger;
        private readonly IFeaturesDbContext _featuresDbContext;

        public UpdateBetaTesterCommandHandler(IFeaturesDbContext featuresDbContext, ILogger logger = null)
        {
            _featuresDbContext = featuresDbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateBetaTesterCommand request, CancellationToken cancellationToken)
        {
            var tester =
                await _featuresDbContext.BetaTesters.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (tester == null)
            {
                throw new BetaTesterNotFoundException(request.Id.ToString());
            }

            tester.IsEnabled = request.IsEnabled;
            await _featuresDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}