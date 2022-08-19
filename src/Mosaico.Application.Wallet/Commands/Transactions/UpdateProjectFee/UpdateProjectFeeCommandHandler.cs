using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateProjectFee
{
    public class UpdateProjectFeeCommandHandler : IRequestHandler<UpdateProjectFeeCommand>
    {
        private readonly IWalletDbContext _walletDbContext;

        public UpdateProjectFeeCommandHandler(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<Unit> Handle(UpdateProjectFeeCommand request, CancellationToken cancellationToken)
        {
            var projectFee = await _walletDbContext.FeeToProjects.FirstOrDefaultAsync(f => f.ProjectId == request.ProjectId, cancellationToken);
            if (projectFee == null)
            {
                projectFee = new FeeToProject
                {
                    FeePercentage = request.FeePercentage,
                    ProjectId = request.ProjectId
                };
                _walletDbContext.FeeToProjects.Add(projectFee);
            }
            else
            {
                projectFee.FeePercentage = request.FeePercentage;
            }

            await _walletDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}