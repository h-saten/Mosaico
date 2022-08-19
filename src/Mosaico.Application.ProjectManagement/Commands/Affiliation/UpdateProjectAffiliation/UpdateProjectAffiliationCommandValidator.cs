using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.UpdateProjectAffiliation
{
    public class UpdateProjectAffiliationCommandValidator : AbstractValidator<UpdateProjectAffiliationCommand>
    {
        public UpdateProjectAffiliationCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }   
    }
}