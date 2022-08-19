using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetStageAuthorizationCode
{
    public class GetStageAuthorizationCodeQueryValidator : AbstractValidator<GetStageAuthorizationCodeQuery>
    {
        public GetStageAuthorizationCodeQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.StageId).NotEmpty();
        }
    }
}