using System;
using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.UploadCompanyLogo
{
    public class UploadCompanyLogoCommandValidator : AbstractValidator<UploadCompanyLogoCommand>
    {
        public UploadCompanyLogoCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.CompanyId).Must(x => x != Guid.Empty);
        }
    }
}