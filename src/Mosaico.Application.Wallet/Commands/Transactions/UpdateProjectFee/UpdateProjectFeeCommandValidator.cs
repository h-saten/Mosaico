using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateProjectFee
{
    public class UpdateProjectFeeCommandValidator : AbstractValidator<UpdateProjectFeeCommand>
    {
        public UpdateProjectFeeCommandValidator()
        {
            RuleFor(t => t.FeePercentage).GreaterThan(0).LessThanOrEqualTo(100);
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}