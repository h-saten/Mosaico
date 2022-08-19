using System.Linq;
using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.UploadCompanyDocuments
{
    public class UploadCompanyDocumentsCommandValidator : AbstractValidator<UploadCompanyDocumentsCommand>
    {
        public UploadCompanyDocumentsCommandValidator()
        {
            RuleFor(t => t.Content).NotEmpty();
            RuleFor(t => t.Language).NotEmpty().Must(l => Mosaico.Base.Constants.Languages.All.Contains(l));
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}
