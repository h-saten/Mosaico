using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.CreateCompanyDocument
{
    public class CreateCompanyDocumentCommandValidator : AbstractValidator<CreateCompanyDocumentCommand>
    {
        public CreateCompanyDocumentCommandValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.CompanyId).NotNull().NotEmpty();
        }
    }
}
