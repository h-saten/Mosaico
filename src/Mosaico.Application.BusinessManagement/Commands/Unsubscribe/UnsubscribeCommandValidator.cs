using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.Unsubscribe
{
    public class UnsubscribeCommandValidator : AbstractValidator<UnsubscribeCommand>
    {
        public UnsubscribeCommandValidator()
        {
            RuleFor(c => c.CompanyId).NotEmpty();
        }
    }
}