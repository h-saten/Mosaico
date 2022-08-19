using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.DeleteProjectDocument
{
    public class DeleteProjectDocumentCommandValidator : AbstractValidator<DeleteProjectDocumentCommand>
    {
        public DeleteProjectDocumentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
