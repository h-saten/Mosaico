using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Project.GetProjectInvestor
{
    public class GetProjectInvestorQueryValidator : AbstractValidator<GetProjectInvestorQuery>
    {
        public GetProjectInvestorQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}