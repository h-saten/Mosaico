using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteInvestmentPackage
{
    public class DeleteInvestmentPackageCommandValidator : AbstractValidator<DeleteInvestmentPackageCommand>
    {
        public DeleteInvestmentPackageCommandValidator()
        {
            RuleFor(t => t.PageId).NotEmpty();
            RuleFor(t => t.InvestmentPackageId).NotEmpty();
        }
    }
}