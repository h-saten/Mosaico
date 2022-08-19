using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.UpdateCompanyDocument
{
    public class UpdateCompanyDocumentCommandValidator : AbstractValidator<UpdateCompanyDocumentCommand>
    {
        public UpdateCompanyDocumentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.Title).NotNull().NotEmpty();
        }
    }
}
