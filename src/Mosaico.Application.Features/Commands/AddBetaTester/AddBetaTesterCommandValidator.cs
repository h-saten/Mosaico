using FluentValidation;

namespace Mosaico.Application.Features.Commands.AddBetaTester
{
    public class AddBetaTesterCommandValidator : AbstractValidator<AddBetaTesterCommand>
    {
        public AddBetaTesterCommandValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
        }        
    }
}