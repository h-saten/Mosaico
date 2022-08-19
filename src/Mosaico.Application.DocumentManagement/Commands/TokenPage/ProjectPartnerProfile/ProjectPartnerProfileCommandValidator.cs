using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.ProjectPartnerProfile
{
    public class ProjectPartnerProfileCommandValidator : AbstractValidator<ProjectPartnerProfileCommand>
    {
        public ProjectPartnerProfileCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
        }
    }
}
