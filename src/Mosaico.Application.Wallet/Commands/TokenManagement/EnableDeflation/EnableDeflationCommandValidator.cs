using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.EnableDeflation
{
    public class EnableDeflationCommandValidator : AbstractValidator<EnableDeflationCommand>
    {
        public EnableDeflationCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty().WithErrorCode("INVALID_TOKEN");
        }
    }
}