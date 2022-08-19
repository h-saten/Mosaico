using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.DeleteCompanyTeamMember
{
    public class DeleteCompanyTeamMemberCommandValidator : AbstractValidator<DeleteCompanyTeamMemberCommand>
    {
        public DeleteCompanyTeamMemberCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}