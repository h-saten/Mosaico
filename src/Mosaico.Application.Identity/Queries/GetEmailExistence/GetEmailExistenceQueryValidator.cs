using FluentValidation;

namespace Mosaico.Application.Identity.Queries.GetEmailExistence
{
    public class GetEmailExistenceQueryValidator : AbstractValidator<GetEmailExistenceQuery>
    {
        public GetEmailExistenceQueryValidator()
        {
            RuleFor(c => c.Email).NotEmpty();
        }
    }
}