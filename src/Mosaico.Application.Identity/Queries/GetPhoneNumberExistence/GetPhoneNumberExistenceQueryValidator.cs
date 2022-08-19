using FluentValidation;

namespace Mosaico.Application.Identity.Queries.GetPhoneNumberExistence
{
    public class GetPhoneNumberExistenceQueryValidator : AbstractValidator<GetPhoneNumberExistenceQuery>
    {
        public GetPhoneNumberExistenceQueryValidator()
        {
            RuleFor(c => c.PhoneNumber).NotEmpty();
        }
    }
}