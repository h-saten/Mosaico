using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteInvestmentPackage
{
    public class DeleteInvestmentPackageCommandHandler : IRequestHandler<DeleteInvestmentPackageCommand>
    {
        private readonly IProjectDbContext _projectDbContext;

        public DeleteInvestmentPackageCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Unit> Handle(DeleteInvestmentPackageCommand request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages
                .Include(p => p.InvestmentPackages)
                .FirstOrDefaultAsync(t => t.Id == request.PageId, cancellationToken);
            if (page == null)
            {
                throw new PageNotFoundException(request.PageId.ToString());
            }

            var investmentPackage = page.InvestmentPackages.FirstOrDefault(t => t.Id == request.InvestmentPackageId);
            if (investmentPackage == null)
            {
                throw new InvestmentPackageNotFoundException(request.InvestmentPackageId.ToString());
            }

            _projectDbContext.InvestmentPackages.Remove(investmentPackage);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}