using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageForUpdate
{
    public class GetPageForUpdateQueryValidator : AbstractValidator<GetPageForUpdateQuery>
    {
        public GetPageForUpdateQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }
}