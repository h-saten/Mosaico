using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.EnablePartner
{
    public class EnablePartnerCommandValidator : AbstractValidator<EnablePartnerCommand>
    {
    public EnablePartnerCommandValidator()
    {
        RuleFor(t => t.PartnerId).NotEmpty();
        RuleFor(t => t.ProjectId).NotEmpty();
    }
    }
}