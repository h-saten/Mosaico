using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.DeleteCompanyDocument
{
    public class DeleteCompanyDocumentCommandValidator : AbstractValidator<DeleteCompanyDocumentCommand>
    {
        public DeleteCompanyDocumentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
