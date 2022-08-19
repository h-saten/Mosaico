using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetProjectWallet
{
    public class GetProjectWalletQueryValidator : AbstractValidator<GetProjectWalletQuery>
    {
        public GetProjectWalletQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}