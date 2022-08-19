using FluentValidation;

namespace Mosaico.Application.Identity.Queries.GetReCaptchaSiteVerify
{
    public class GetReCaptchaSiteVerifyQueryValidator : AbstractValidator<GetReCaptchaSiteverifyQuery>
    {
        public GetReCaptchaSiteVerifyQueryValidator()
        {
            RuleFor(c => c.Response).NotEmpty();
        }
    }
}