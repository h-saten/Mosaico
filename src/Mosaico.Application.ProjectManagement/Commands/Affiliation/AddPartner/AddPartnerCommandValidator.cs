using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.AddPartner
{
    public class AddPartnerCommandValidator : AbstractValidator<AddPartnerCommand>
    {
        public AddPartnerCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.Email).NotEmpty();
        }
    }
}