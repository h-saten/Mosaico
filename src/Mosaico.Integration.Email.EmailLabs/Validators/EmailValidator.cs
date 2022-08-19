using FluentValidation;

namespace Mosaico.Integration.Email.EmailLabs.Validators
{
    public class EmailValidator : AbstractValidator<Abstraction.Email>
    {
        public EmailValidator()
        {
            //email labs limitation
            RuleFor(e => e.Recipients).NotEmpty().Must(r => r.Count <= 200);
            //email labs limitation
            RuleFor(e => e.Subject).NotEmpty().Length(1, 128);
            //email labs limitation
            RuleFor(e => e.FromName).NotEmpty().Length(1, 128);
            RuleFor(e => e.FromEmail).NotEmpty();
            RuleFor(e => e.Html).NotEmpty();
        }
    }
}