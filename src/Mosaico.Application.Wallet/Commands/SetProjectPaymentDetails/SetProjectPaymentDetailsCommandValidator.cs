using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.SetProjectPaymentDetails
{
    public class SetProjectPaymentDetailsCommandValidator : AbstractValidator<SetProjectPaymentDetailsCommand>
    {
        public SetProjectPaymentDetailsCommandValidator()
        {
            RuleFor(t => t.Account).NotEmpty();
            RuleFor(t => t.Swift).NotEmpty();
            RuleFor(t => t.AccountAddress).NotEmpty();
            RuleFor(t => t.BankName).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}