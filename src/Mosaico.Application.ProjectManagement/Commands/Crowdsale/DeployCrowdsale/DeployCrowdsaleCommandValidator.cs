using FluentValidation;
using Mosaico.Application.ProjectManagement.Commands.Crowdsale.DeployCrowdsale;

namespace Mosaico.Application.ProjectManagement.Commands.DeployCrowdsale
{
    public class DeployCrowdsaleCommandValidator : AbstractValidator<DeployCrowdsaleCommand>
    {
        public DeployCrowdsaleCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}