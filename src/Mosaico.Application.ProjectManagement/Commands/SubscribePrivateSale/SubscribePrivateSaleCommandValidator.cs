using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.SubscribePrivateSale
{
    public class SubscribePrivateSaleCommandValidator : AbstractValidator<SubscribePrivateSaleCommand>
    {
        public SubscribePrivateSaleCommandValidator()
        {
            RuleFor(t => t.AuthorizationCode).NotEmpty();
        }
    }
}