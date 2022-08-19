using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UploadProjectIntroVideo
{
    public class UploadProjectIntroVideoCommandValidator: AbstractValidator<UploadProjectIntroVideoCommand>
    {
        public UploadProjectIntroVideoCommandValidator()
        {
            RuleFor(c => c.Content).NotEmpty();
            RuleFor(c => c.PageId).NotEmpty();
        }
    }
}
