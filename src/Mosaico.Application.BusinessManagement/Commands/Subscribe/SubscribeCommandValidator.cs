using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.Subscribe
{
    public class SubscribeCommandValidator : AbstractValidator<SubscribeCommand>
    {
        public SubscribeCommandValidator()
        {
            RuleFor(c => c.CompanyId).NotEmpty();
            RuleFor(c => c).Must(command =>
                !string.IsNullOrWhiteSpace(command?.Email) && !string.IsNullOrWhiteSpace(command?.UserId));
        }
    }
}