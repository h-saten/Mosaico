using FluentValidation;

namespace Mosaico.Application.Features.Commands.UpsertSetting
{
    public class UpsertSettingCommandValidator : AbstractValidator<UpsertSettingCommand>
    {
        public UpsertSettingCommandValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(e => e.Value).NotEmpty();
                RuleFor(e => e.FeatureName).NotEmpty();
            });
        }
    }
}