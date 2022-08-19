using System.Linq;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UploadProjectDocuments
{
    public class UploadProjectDocumentsCommandValidator : AbstractValidator<UploadProjectDocumentsCommand>
    {
        public UploadProjectDocumentsCommandValidator()
        {
            RuleFor(t => t.Content).NotEmpty();
            RuleFor(t => t.Language).NotEmpty().Must(l => Mosaico.Base.Constants.Languages.All.Contains(l));
            RuleFor(t => t.Type).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}