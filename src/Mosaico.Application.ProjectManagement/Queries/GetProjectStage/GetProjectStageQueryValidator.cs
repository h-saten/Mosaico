using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectStage
{
    public class GetProjectStageQueryValidator : AbstractValidator<GetProjectStageQuery>
    {
        public GetProjectStageQueryValidator()
        {
            RuleFor(s => s.ProjectId).NotNull().NotEmpty();
            RuleFor(s => s.StageId).NotNull().NotEmpty();
        }
    }
}