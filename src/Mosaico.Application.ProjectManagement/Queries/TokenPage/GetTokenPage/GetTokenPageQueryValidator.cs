using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPage
{
    public class GetTokenPageQueryValidator : AbstractValidator<GetTokenPageQuery>
    {
        public GetTokenPageQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }
}