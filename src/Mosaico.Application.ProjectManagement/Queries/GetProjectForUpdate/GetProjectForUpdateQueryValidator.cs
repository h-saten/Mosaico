using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectForUpdate
{
    public class GetProjectForUpdateQueryValidator : AbstractValidator<GetProjectForUpdateQuery>
    {
        public GetProjectForUpdateQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }
}