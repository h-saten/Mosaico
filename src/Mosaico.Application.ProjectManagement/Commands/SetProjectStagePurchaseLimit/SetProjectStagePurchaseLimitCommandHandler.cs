using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.SetProjectStagePurchaseLimit
{
    public class SetProjectStagePurchaseLimitCommandHandler : IRequestHandler<SetProjectStagePurchaseLimitCommand>
    {
        private readonly IProjectDbContext _projectDbContext;

        public SetProjectStagePurchaseLimitCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Unit> Handle(SetProjectStagePurchaseLimitCommand request, CancellationToken cancellationToken)
        {
            var stage = await _projectDbContext.Stages.Include(s => s.PurchaseLimits).FirstOrDefaultAsync(t => t.ProjectId == request.ProjectId && t.Id == request.StageId, cancellationToken: cancellationToken);
            if (stage == null) throw new StageNotFoundException(request.ProjectId, request.StageId);
            var limit = stage.PurchaseLimits.FirstOrDefault(l => l.PaymentMethod == request.PaymentMethod);
            if (limit == null)
            {
                limit = new StagePurchaseLimit
                {
                    StageId = stage.Id,
                    Stage = stage,
                    PaymentMethod = request.PaymentMethod
                };
                await _projectDbContext.StagePurchaseLimits.AddAsync(limit, cancellationToken);
            }
            if (request.MaximumPurchase > stage.TokensSupply)
            {
                throw new InvalidStageIdException();
            }

            limit.MaximumPurchase = request.MaximumPurchase;
            limit.MinimumPurchase = request.MinimumPurchase;
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}