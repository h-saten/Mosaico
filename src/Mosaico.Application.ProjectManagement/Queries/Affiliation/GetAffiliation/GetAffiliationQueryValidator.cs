using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.Affiliation.GetAffiliation
{
    public class GetAffiliationQueryValidator : AbstractValidator<GetAffiliationQuery>
    {
        public GetAffiliationQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}