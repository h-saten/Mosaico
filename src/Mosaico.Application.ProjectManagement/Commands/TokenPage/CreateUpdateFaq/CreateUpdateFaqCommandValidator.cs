using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateFaq
{
    public class CreateUpdateFaqCommandValidator : AbstractValidator<CreateUpdateFaqCommand>
    {
        public CreateUpdateFaqCommandValidator()
        {
            RuleFor(c => c.Content).NotEmpty().Length(3, 500);
            RuleFor(c => c.Title).NotEmpty().Length(3, 100);
            RuleFor(c => c.Order).GreaterThan(0);
            RuleFor(c => c.PageId).NotEmpty();
        }
    }
}