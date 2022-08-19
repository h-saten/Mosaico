using FluentValidation;
using Mosaico.Secrets.KeyVault.Configurations;

namespace Mosaico.Secrets.KeyVault.Validators
{
    public class KeyVaultCertificateConfigurationValidator : AbstractValidator<KeyVaultConfiguration>
    {
        public KeyVaultCertificateConfigurationValidator()
        {
            RuleFor(c => c.Endpoint).NotEmpty();
        }
    }
}