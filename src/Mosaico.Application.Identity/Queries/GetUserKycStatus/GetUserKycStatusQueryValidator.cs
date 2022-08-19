using FluentValidation;

namespace Mosaico.Application.Identity.Queries.GetUserKycStatus
{
    public class GetUserKycStatusQueryValidator : AbstractValidator<GetUserKycStatusQuery>
    {
        public GetUserKycStatusQueryValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}