using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.Vote
{
    public class VoteCommandValidator : AbstractValidator<VoteCommand>
    {
        public VoteCommandValidator()
        {
            RuleFor(t => t.CompanyId).NotEmpty();
            RuleFor(t => t.ProposalId).NotEmpty();
        }
    }
}