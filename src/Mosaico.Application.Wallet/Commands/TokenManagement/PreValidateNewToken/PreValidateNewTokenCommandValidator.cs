using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using FluentValidation.Results;
using Mosaico.Application.Wallet.Commands.TokenManagement.CreateToken;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.PreValidateNewToken
{
    public class PreValidateNewTokenCommandValidator : CreateTokenCommandValidator, IValidator<PreValidateNewTokenCommand>
    {
        public PreValidateNewTokenCommandValidator(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {
        }

        public ValidationResult Validate(PreValidateNewTokenCommand instance)
        {
            return Validate(instance);
        }

        public Task<ValidationResult> ValidateAsync(PreValidateNewTokenCommand instance,
            CancellationToken cancellation = new())
        {
            return ValidateAsync(instance, cancellation);
        }
    }
}