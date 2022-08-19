using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetTokenomics
{
    public class GetTokenomicsQueryValidator : AbstractValidator<GetTokenomicsQuery>
    {
        public GetTokenomicsQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}