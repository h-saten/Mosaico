using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Project.GetProjectFee
{
    public class GetProjectFeeQueryHandler : IRequestHandler<GetProjectFeeQuery, decimal>
    {
        private readonly IWalletDbContext _walletDbContext;

        public GetProjectFeeQueryHandler(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<decimal> Handle(GetProjectFeeQuery request, CancellationToken cancellationToken)
        {
            var projectFee =
                await _walletDbContext.FeeToProjects.FirstOrDefaultAsync(f => f.ProjectId == request.ProjectId,
                    cancellationToken);
            return projectFee?.FeePercentage ?? 7;
        }
    }
}