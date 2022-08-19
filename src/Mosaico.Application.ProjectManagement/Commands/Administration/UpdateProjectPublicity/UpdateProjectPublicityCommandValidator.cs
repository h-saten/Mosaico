using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Administration.UpdateProjectPublicity
{
    public class UpdateProjectPublicityCommandValidator : AbstractValidator<UpdateProjectPublicityCommand>
    {
        public UpdateProjectPublicityCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}