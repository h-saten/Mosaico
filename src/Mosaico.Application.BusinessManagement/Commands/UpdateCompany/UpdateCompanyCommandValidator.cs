using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.UpdateCompany
{
    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            RuleSet("default", () =>
            {
            });
        }
    }
}