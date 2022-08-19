using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.ApplyUserReference
{
    public class ApplyUserReferenceCommandValidator : AbstractValidator<ApplyUserReferenceCommand>
    {
        public ApplyUserReferenceCommandValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.RefCode).NotEmpty();
        }
    }
}