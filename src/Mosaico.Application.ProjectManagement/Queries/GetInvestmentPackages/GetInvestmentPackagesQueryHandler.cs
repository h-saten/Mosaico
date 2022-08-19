using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Extensions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Queries.GetInvestmentPackages
{
    public class GetInvestmentPackagesQueryHandler : IRequestHandler<GetInvestmentPackagesQuery, GetInvestmentPackagesQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;

        public GetInvestmentPackagesQueryHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<GetInvestmentPackagesQueryResponse> Handle(GetInvestmentPackagesQuery request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages
                .Include(t => t.InvestmentPackages).ThenInclude(i => i.Translations)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (page == null)
            {
                throw new PageNotFoundException(request.Id.ToString());
            }

            return new GetInvestmentPackagesQueryResponse
            {
                Packages = page.InvestmentPackages?.OrderBy(ip => ip.TokenAmount).Select(i => i.ToFlatDTO(request.Language)).ToList()
            };
        }
    }
}