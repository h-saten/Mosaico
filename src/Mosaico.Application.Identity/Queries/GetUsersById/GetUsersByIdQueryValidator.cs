using FluentValidation;

namespace Mosaico.Application.Identity.Queries.GetUsersById
{
    public class GetUsersByIdQueryValidator : AbstractValidator<GetUsersByIdQuery>
    {
        public GetUsersByIdQueryValidator()
        {
            RuleFor(t => t.UsersId.Count).GreaterThan(0);
        }
    }
}