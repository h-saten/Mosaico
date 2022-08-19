using System.Linq;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UploadCertificateBackground
{
    public class UploadCertificateBackgroundValidator : AbstractValidator<UploadCertificateBackgroundCommand>
    {
        public UploadCertificateBackgroundValidator()
        {
            RuleFor(t => t.Content).NotEmpty();
            RuleFor(t => t.Language).NotEmpty().Must(l => Mosaico.Base.Constants.Languages.All.Contains(l));
            RuleFor(t => t.OriginalFileName).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}