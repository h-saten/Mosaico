using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.UpdatePartner
{
    public class UpdatePartnerCommandValidator : AbstractValidator<UpdatePartnerCommand>
    {
        public UpdatePartnerCommandValidator()
        {
            RuleFor(t => t.PartnerId).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.RewardPercentage).GreaterThan(0);
            RuleFor(t => t.RewardPercentage).LessThan(100);
        }
    }
}