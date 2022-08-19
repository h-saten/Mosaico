using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyProposals
{
    public class GetCompanyProposalsQueryValidator : AbstractValidator<GetCompanyProposalsQuery>
    {
        public GetCompanyProposalsQueryValidator()
        {
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}