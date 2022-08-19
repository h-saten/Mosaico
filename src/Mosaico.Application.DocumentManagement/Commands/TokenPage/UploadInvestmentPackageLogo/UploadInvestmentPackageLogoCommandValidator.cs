using System;
using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadInvestmentPackageLogo
{
    public class UploadInvestmentPackageLogoCommandValidator : AbstractValidator<UploadInvestmentPackageLogoCommand>
    {
        public UploadInvestmentPackageLogoCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.PageId).Must(x => x != Guid.Empty);
            RuleFor(x => x.InvestmentPackageId).Must(x => x != Guid.Empty);
        }
    }
}