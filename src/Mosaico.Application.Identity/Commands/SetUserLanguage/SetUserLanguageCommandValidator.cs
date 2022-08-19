using System.Linq;
using FluentValidation;

namespace Mosaico.Application.Identity.Commands.SetUserLanguage
{
    public class SetUserLanguageCommandValidator : AbstractValidator<SetUserLanguageCommand>
    {
        public SetUserLanguageCommandValidator()
        {
            RuleFor(c => c.Language).NotEmpty().Must(l => Base.Constants.Languages.All.Contains(l));
        }
    }
}