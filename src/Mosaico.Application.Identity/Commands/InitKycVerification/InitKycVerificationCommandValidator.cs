using FluentValidation;

namespace Mosaico.Application.Identity.Commands.InitKycVerification
{
    public class InitKycVerificationCommandValidator : AbstractValidator<InitKycVerificationCommand>
    {
        public InitKycVerificationCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.Provider).NotEmpty();
            RuleFor(t => t.Provider).Must(t => t == "PASSBASE");
        }
    }
}