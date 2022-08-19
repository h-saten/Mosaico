using FluentValidation;
using Mosaico.Storage.AzureBlobStorage.Configurations;

namespace Mosaico.Storage.AzureBlobStorage.Validators
{
    public class AzureBlobStorageConfigValidator : AbstractValidator<AzureBlobStorageConfiguration>
    {
        public AzureBlobStorageConfigValidator()
        {
            RuleFor(c => c.ConnectionString).NotEmpty().WithMessage($"Blob Storage is misconfigured");
            RuleFor(c => c.EndpointUrl).NotEmpty().WithMessage($"Blob Storage is misconfigured");
        }
    }
}