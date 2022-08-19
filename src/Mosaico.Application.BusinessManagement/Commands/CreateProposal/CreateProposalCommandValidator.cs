using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.CreateProposal
{
    public class CreateProposalCommandValidator : AbstractValidator<CreateProposalCommand>
    {
        public CreateProposalCommandValidator()
        {
            RuleFor(t => t.Title).NotEmpty().Length(5, 30);
            RuleFor(t => t.CompanyId).NotEmpty();
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(t => t.Description).NotEmpty().Length(5, 500);
            RuleFor(t => t.QuorumThreshold).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}