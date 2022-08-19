using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UpdateProjectDocument
{
    public class UpdateProjectDocumentCommandValidator : AbstractValidator<UpdateProjectDocumentCommand>
    {
        public UpdateProjectDocumentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.Title).NotNull().NotEmpty();
        }
    }
}
