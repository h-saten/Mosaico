using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Commands.UpdateSocialMediaLinks
{
    public class UpdateSocialMediaLinksCommandValidator : AbstractValidator<UpdateSocialMediaLinksCommand>
    {
        public UpdateSocialMediaLinksCommandValidator()
        {
            var reg = Constants.RegEx.UrlRegEx;
            RuleSet("default", () =>
            {
                RuleFor(e => e.CompanyId).NotEmpty();
                RuleFor(e => e.Telegram).Matches(reg).WithErrorCode("ERROR_MESSAGES.invalid_telegram");
                RuleFor(e => e.Youtube).Matches(reg).WithErrorCode("ERROR_MESSAGES.invalid_youtube");
                RuleFor(e => e.LinkedIn).Matches(reg).WithErrorCode("ERROR_MESSAGES.invalid_linkedin");
                RuleFor(e => e.Facebook).Matches(reg).WithErrorCode("ERROR_MESSAGES.invalid_facebook");
                RuleFor(e => e.Twitter).Matches(reg).WithErrorCode("ERROR_MESSAGES.invalid_twitter");
                RuleFor(e => e.Instagram).Matches(reg).WithErrorCode("ERROR_MESSAGES.invalid_instagram");
                RuleFor(e => e.Medium).Matches(reg).WithErrorCode("ERROR_MESSAGES.invalid_medium");
            });
        }
    }
}