using System.Linq;
using FluentValidation;

namespace Mosaico.Application.Identity.Queries.FindUser
{
    public class FindUserQueryValidator : AbstractValidator<FindUserQuery>
    {
        public FindUserQueryValidator()
        {
            RuleFor(c => c.FindBy).NotEmpty();
            RuleFor(c => c.Value).NotEmpty();
            RuleFor(c => c.FindBy).Must(v => Constants.UserFindFields.All.Contains(v));
        }
    }
}