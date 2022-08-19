using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertInvestmentPackage
{
    public class UpsertInvestmentPackageCommandHandler : IRequestHandler<UpsertInvestmentPackageCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;

        public UpsertInvestmentPackageCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Guid> Handle(UpsertInvestmentPackageCommand request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages
                .Include(t => t.InvestmentPackages).ThenInclude(t => t.Translations)
                .FirstOrDefaultAsync(t => t.Id == request.PageId, cancellationToken);
            
            if (request.InvestmentPackageId != Guid.Empty)
            {
                var package = page.InvestmentPackages.FirstOrDefault(p => p.Id == request.InvestmentPackageId);
                if (package == null)
                {
                    throw new InvestmentPackageNotFoundException(request.InvestmentPackageId.ToString());
                }
                
                var benefits = request.Benefits?.Select(b => b.Trim());
                var translation = package.GetTranslationInLanguage(request.Language, true);
                if (translation == null)
                {
                    CreateNewTranslation(request, package);
                }
                else
                {
                    translation.Benefits = benefits != null ? string.Join(Domain.ProjectManagement.Constants.InvestmentPackageBenefitSeparator, benefits) : string.Empty;
                    translation.Name = request.Name;
                }

                package.TokenAmount = request.TokenAmount;
                _projectDbContext.InvestmentPackages.Update(package);
                await _projectDbContext.SaveChangesAsync(cancellationToken);
                return package.Id;
            }
            else
            {
                var package = new InvestmentPackage
                {
                    Page = page,
                    PageId = page.Id
                };
                CreateNewTranslation(request, package);
                package.TokenAmount = request.TokenAmount;
                _projectDbContext.InvestmentPackages.Add(package);
                await _projectDbContext.SaveChangesAsync(cancellationToken);
                return package.Id;
            }
        }
        
        private void CreateNewTranslation(UpsertInvestmentPackageCommand request, InvestmentPackage package)
        {
            var benefits = request.Benefits?.Select(b => b.Trim());
            var translation = new InvestmentPackageTranslation
            {
                Benefits = benefits != null
                    ? string.Join(Domain.ProjectManagement.Constants.InvestmentPackageBenefitSeparator, benefits)
                    : string.Empty,
                Language = request.Language,
                Name = request.Name
            };
            package.Translations.Add(translation);
        }
    }
}