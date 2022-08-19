using FluentValidation;
using Mosaico.Domain.BusinessManagement.Abstractions;

namespace Mosaico.Application.BusinessManagement.Commands.CreateVerification
{
    public class CreateVerificationCommandValidator : AbstractValidator<CreateVerificationCommand>
    {
        public void CreateVerificationsCommandValidator(IBusinessDbContext context)
        {
            RuleSet("default", () =>
            {
                RuleFor(e => e.CompanyAddressUrl).NotEmpty();
                RuleFor(e => e.CompanyRegistrationUrl).NotEmpty();
                RuleFor(e => e.Shareholders).NotEmpty();
            });
        }
    }
}