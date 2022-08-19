using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertInvestmentPackage
{
    public class UpsertInvestmentPackageCommandValidator : AbstractValidator<UpsertInvestmentPackageCommand>
    {
        public UpsertInvestmentPackageCommandValidator()
        {
            RuleFor(t => t.PageId).NotEmpty();
            RuleFor(t => t.Benefits).NotNull().Must(t => t.Count > 0 && t.Count <= 5);
            RuleFor(t => t.TokenAmount).GreaterThan(0);
            RuleFor(t => t.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}