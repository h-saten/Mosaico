using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectStages
{
    public class GetProjectStagesQueryValidator : AbstractValidator<GetProjectStagesQuery>
    {
        public GetProjectStagesQueryValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();
        }
    }
}