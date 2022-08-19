using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetCompanyProjects
{
    public class GetCompanyProjectsQueryValidator : AbstractValidator<GetCompanyProjectsQuery>
    {
        public GetCompanyProjectsQueryValidator()
        {
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}