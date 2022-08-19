using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UpdateProjectIntroVideoUrl
{
    public class UpdateProjectIntroVideoUrlCommandValidator: AbstractValidator<UpdateProjectIntroVideoUrlCommand>
    {
        public UpdateProjectIntroVideoUrlCommandValidator()
        {
            RuleFor(c => c.PageId).NotEmpty();
            RuleFor(c => c.VideoUrl).NotEmpty();
        }
    }
}
