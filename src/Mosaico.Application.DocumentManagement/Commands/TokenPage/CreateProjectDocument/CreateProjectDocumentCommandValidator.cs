using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.CreateProjectDocument
{
    public class CreateProjectDocumentCommandValidator : AbstractValidator<CreateProjectDocumentCommand>
    {
        public CreateProjectDocumentCommandValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.ProjectId).NotNull().NotEmpty();
        }
    }
}
