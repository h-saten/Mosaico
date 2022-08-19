using FluentValidation;
using Mosaico.Domain.BusinessManagement.Abstractions;

namespace Mosaico.Application.BusinessManagement.Commands.CreateCompany
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(e => e.CompanyName).NotEmpty();
                RuleFor(e => e.Network).Must(c => Blockchain.Base.Constants.BlockchainNetworks.All.Contains(c));
            });
        }
    }
}