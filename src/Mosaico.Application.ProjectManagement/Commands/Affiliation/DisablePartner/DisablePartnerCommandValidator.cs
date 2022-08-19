using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.DisablePartner
{
    public class DisablePartnerCommandValidator : AbstractValidator<DisabledPartnerCommand>
    {
        public DisablePartnerCommandValidator()
        {
            RuleFor(t => t.PartnerId).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}