using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectSubscribers
{
    public class GetProjectSubscribersQueryValidator : AbstractValidator<GetProjectSubscribersQuery>
    {
        public GetProjectSubscribersQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}