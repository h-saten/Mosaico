using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.Staking.UploadStakingTerms
{
    public class UploadStakingTermsCommandValidator : AbstractValidator<UploadStakingTermsCommand>
    {
        public UploadStakingTermsCommandValidator()
        {
            RuleFor(t => t.StakingPairId).NotEmpty();
            RuleFor(t => t.StakingPairId).NotEmpty();
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
        }
    }
}