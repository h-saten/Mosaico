using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.CompanyDocumentFile
{
    public class CompanyDocumentFileCommandValidator : AbstractValidator<CompanyDocumentFileCommand>
    {
        public CompanyDocumentFileCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
        }
    }
}
