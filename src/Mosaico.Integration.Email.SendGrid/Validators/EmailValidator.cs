using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Integration.Email.SendGridEmail.Validators
{
    public class EmailValidator : AbstractValidator<Abstraction.Email>
    {
        public EmailValidator()
        {
            RuleFor(e => e.Recipients).NotEmpty().Must(r => r.Count <= 200);
            RuleFor(e => e.Subject).NotEmpty().Length(1, 128);
            RuleFor(e => e.FromName).NotEmpty().Length(1, 128);
            RuleFor(e => e.FromEmail).NotEmpty();
            RuleFor(e => e.Html).NotEmpty();
        }
    }
}
