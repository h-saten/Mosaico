using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.UpdateToken
{
    public class UpdateTokenCommandValidator : AbstractValidator<UpdateTokenCommand>
    {
        public UpdateTokenCommandValidator()
        {
            RuleFor(t => t.ContractAddress).NotEmpty();
            RuleFor(t => t.ContractVersion)
                .Must(s => Integration.Blockchain.Ethereum.Constants.TokenContractVersions.All.Contains(s))
                .NotEmpty();
            RuleFor(t => t.OwnerAddress).NotEmpty();
            RuleFor(t => t.TokenId).NotEmpty();
        }
    }
}