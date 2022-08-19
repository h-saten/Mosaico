using System.Text.RegularExpressions;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(e => e.Title).MinimumLength(3).MaximumLength(20);
                RuleFor(c => c.ShortDescription).MinimumLength(3).MaximumLength(180);
                RuleFor(c => c.CompanyId).NotEmpty();
            });
        }
    }
}