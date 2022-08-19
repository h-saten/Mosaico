using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteProjectPartner
{
    public class DeleteProjectPartnerCommandValidator : AbstractValidator<DeleteProjectPartnerCommand>
    {
        public DeleteProjectPartnerCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.PageId).NotEmpty();
        }
    }
}