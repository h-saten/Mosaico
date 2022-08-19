using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.ProjectManagement.Validators
{
    public class ProjectInStatusValidator : AbstractValidator<Project>
    {
        private readonly IBusinessManagementClient _businessManagementClient;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IWalletClient _walletClient;
        
        public ProjectInStatusValidator(IBusinessManagementClient businessManagementClient, IProjectDbContext projectDbContext, IWalletClient walletClient)
        {
            _businessManagementClient = businessManagementClient;
            _projectDbContext = projectDbContext;
            _walletClient = walletClient;
            RuleSet("stages", () =>
            {
                RuleFor(p => p.Stages).NotNull().WithErrorCode("VALIDATION_NO_STAGES").NotEmpty().WithErrorCode("VALIDATION_NO_STAGES");
                RuleForEach(p => p.Stages).SetValidator(new ProjectStageUnderReviewValidator());
            });
            RuleSet("token", () =>
            {
                RuleFor(p => p.TokenId).NotEmpty().WithErrorCode($"VALIDATION_NO_TOKEN");
                RuleFor(p => p.TokenId).MustAsync(async (guid, t) =>
                {
                    if (!guid.HasValue || guid.Value == Guid.Empty) return true;
                    var token = await _walletClient.GetTokenAsync(guid.Value);
                    return token != null && token.Status == TokenStatus.Deployed;
                }).WithErrorCode($"TOKEN_NOT_DEPLOYED");
            });
            // RuleSet("crowdsale", () =>
            // {
            //     RuleFor(p => p.Crowdsale).Must(c => c != null && c.HardCap > 0 && !string.IsNullOrWhiteSpace(c?.ContractAddress)).WithErrorCode($"VALIDATION_NO_CROWDSALE");
            // });
            RuleSet("description", () =>
            {
                RuleFor(p => p.PageId).MustAsync(ValidatePageAboutAsync).WithErrorCode("VALIDATION_NO_DESCRIPTION");
            });
            RuleSet("title", () =>
            {
                RuleFor(p => p.Title).NotNull().WithErrorCode("VALIDATION_NO_TITLE");

            });
            RuleSet("company", () =>
            {
                RuleFor(p => p.CompanyId).NotEmpty().WithErrorCode("VALIDATION_NO_COMPANY");
                RuleFor(p => p.CompanyId).MustAsync(ValidateCompanyAsync).WithErrorCode("VALIDATION_NOT_FILLED_COMPANY");
            });
            RuleSet("pagecover", () =>
            {
                RuleFor(p => p.PageId).MustAsync(ValidateCoverAsync).WithErrorCode("VALIDATION_NO_PAGE_COVER");
            });
            RuleSet("shortDescription", () =>
            {
                RuleFor(p => p.PageId).MustAsync(ValidatePageShortDescription).WithErrorCode("VALIDATION_NO_SHORT_DESCRIPTION");
            });
            RuleSet("faq", () =>
            {
                RuleFor(p => p.PageId).MustAsync(ValidatePageFAQAsync).WithErrorCode("VALIDATION_NO_FAQ");
            });
            RuleSet("documents", () =>
            {
                RuleFor(p => p.Id).MustAsync(ValidateProjectDocuments).WithErrorCode("VALIDATION_NO_DOCUMENTS");
            });
            RuleSet("team", () =>
            {
                RuleFor(p => p.PageId).MustAsync(ValidatePageTeamAsync).WithErrorCode("VALIDATION_NO_TEAM");
            });
            RuleSet("social-links", () =>
            {
                RuleFor(p => p.PageId).MustAsync(ValidatePageSocialLinksAsync).WithErrorCode("VALIDATION_NO_SOCIAL_MEDIA");
            });
        }

        private async Task<bool> ValidateHardAndSoftCapAsync(Guid? crowdSaleId, CancellationToken t = new CancellationToken())
        {
            if (crowdSaleId.HasValue)
            {
                var crowdsale = await _projectDbContext.Crowdsales.FirstOrDefaultAsync(t => t.Id == crowdSaleId.Value, t);
                if (crowdsale != null)
                {
                    return crowdsale.HardCap > 0 && crowdsale.SoftCap > 0;
                }
            }
            
            return false;
        }

        private Task<bool> ValidatePageShortDescription(Guid? pageId, CancellationToken t = new CancellationToken())
        {
            return pageId.HasValue ? _projectDbContext.ShortDescriptions.AnyAsync(pc => pc.PageId == pageId, t) : Task.FromResult(false);
        }

        private Task<bool> ValidatePageFAQAsync(Guid? pageId, CancellationToken t = new CancellationToken())
        {
            return pageId.HasValue
                ? _projectDbContext.PageFaqs.AnyAsync(pf => pf.PageId == pageId, t)
                : Task.FromResult(false);
        }
        
        private Task<bool> ValidatePageAboutAsync(Guid? pageId, CancellationToken t = new CancellationToken())
        {
            return pageId.HasValue
                ? _projectDbContext.AboutPages.AnyAsync(pf => pf.PageId == pageId, t)
                : Task.FromResult(false);
        }
        
        private Task<bool> ValidatePageTeamAsync(Guid? pageId, CancellationToken t = new CancellationToken())
        {
            return pageId.HasValue
                ? _projectDbContext.PageTeamMembers.AnyAsync(pf => pf.PageId == pageId, t)
                : Task.FromResult(false);
        }
        
        private Task<bool> ValidatePageSocialLinksAsync(Guid? pageId, CancellationToken t = new CancellationToken())
        {
            return pageId.HasValue
                ? _projectDbContext.PageSocialMediaLinks.AnyAsync(pf => pf.PageId == pageId, t)
                : Task.FromResult(false);
        }

        private Task<bool> ValidateCoverAsync(Guid? pageId, CancellationToken t = new CancellationToken())
        {
            return pageId.HasValue ? _projectDbContext.PageCovers.AnyAsync(pc => pc.PageId == pageId, t) : Task.FromResult(false);
        }

        private async Task<bool> ValidateProjectDocuments(Guid projectId, CancellationToken t = new CancellationToken())
        {
            var documents = await _projectDbContext.Documents.Include(d => d.Type)
                .Where(d => d.ProjectId == projectId).ToListAsync(t);
            return documents.Any(d => d.Type.Key == Domain.ProjectManagement.Constants.DocumentTypes.Whitepaper) &&
                   documents.Any(d => d.Type.Key == Domain.ProjectManagement.Constants.DocumentTypes.PrivacyPolicy) &&
                   documents.Any(d => d.Type.Key == Domain.ProjectManagement.Constants.DocumentTypes.TermsAndConditions);
        }

        private async Task<bool> ValidateCompanyAsync(Guid? companyId, CancellationToken t = new CancellationToken())
        {
            if (companyId.HasValue)
            {
                var company = await _businessManagementClient.GetCompanyAsync(companyId.Value, t);
                if (company != null)
                {
                    return !string.IsNullOrWhiteSpace(company.Country) &&
                           !string.IsNullOrWhiteSpace(company.Street)
                           && !string.IsNullOrWhiteSpace(company.PostalCode) &&
                           !string.IsNullOrWhiteSpace(company.Email) && !string.IsNullOrWhiteSpace(company.PhoneNumber);
                }
            }

            return false;
        }
    }
}