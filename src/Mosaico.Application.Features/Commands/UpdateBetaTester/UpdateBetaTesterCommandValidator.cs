using FluentValidation;

namespace Mosaico.Application.Features.Commands.UpdateBetaTester
{
    public class UpdateBetaTesterCommandValidator : AbstractValidator<UpdateBetaTesterCommand>
    {
        public UpdateBetaTesterCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }
}