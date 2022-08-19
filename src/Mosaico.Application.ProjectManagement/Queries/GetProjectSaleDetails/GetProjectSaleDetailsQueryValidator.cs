using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectSaleDetails
{
    public class GetProjectSaleDetailsQueryValidator : AbstractValidator<GetProjectSaleDetailsQuery>
    {
        public GetProjectSaleDetailsQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(d => d.UniqueIdentifier).NotEmpty();
            });
        }
    }
}