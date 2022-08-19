using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.LeaveCompany
{
    public class LeaveCompanyCommandValidator : AbstractValidator<LeaveCompanyCommand>
    {
        public LeaveCompanyCommandValidator()
        {
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}