using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Rating.Like
{
    public class LikeProjectCommandValidator : AbstractValidator<LikeProjectCommand>
    {
        public LikeProjectCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}