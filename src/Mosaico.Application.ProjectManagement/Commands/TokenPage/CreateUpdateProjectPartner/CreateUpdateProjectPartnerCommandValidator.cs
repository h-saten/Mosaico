using FluentValidation;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateProjectPartner
{
    public class CreateUpdateProjectPartnerCommandValidator : AbstractValidator<CreateUpdateProjectPartnerCommand>
    {
        public CreateUpdateProjectPartnerCommandValidator(IProjectDbContext context)
        {
            RuleSet("default", () =>
            {
                RuleFor(e => e.Name).NotEmpty();
                RuleFor(e => e.Position).NotEmpty();
                RuleFor(e => e.PageId).NotEmpty();
            });
        }
    }
}
