using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.ImportTokenDetails
{
    public class ImportTokenDetailsQueryValidator : AbstractValidator<ImportTokenDetailsQuery>
    {
        public ImportTokenDetailsQueryValidator()
        {
            RuleFor(q => q.ContractAddress).NotEmpty().NotNull();
        }
    }
}