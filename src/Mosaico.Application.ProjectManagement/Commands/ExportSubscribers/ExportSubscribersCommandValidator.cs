using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.ExportSubscribers
{
    public class ExportSubscribersCommandValidator : AbstractValidator<ExportSubscribersCommand>
    {
        public ExportSubscribersCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}