using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.ImportToken
{
    public class ImportTokenCommandValidator : AbstractValidator<ImportTokenCommand>
    {
        private readonly ILifetimeScope _lifetimeScope;
        public ImportTokenCommandValidator(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            RuleFor(t => t.Name).MinimumLength(3).MaximumLength(20).WithErrorCode("INVALID_TOKEN_NAME");
            RuleFor(t => t.Symbol).MinimumLength(3).MaximumLength(6).WithErrorCode("INVALID_TOKEN_SYMBOL");
            RuleFor(t => t.Network).Must(n => Blockchain.Base.Constants.BlockchainNetworks.All.Contains(n)).WithErrorCode("INVALID_NETWORK");
            RuleFor(t => t.TokenType).Must(n => Domain.Wallet.Constants.TokenType.All.Contains(n)).WithErrorCode("INVALID_TYPE");
            RuleFor(t => t.Decimals).Equal(18).WithErrorCode("INVALID_DECIMALS");
            RuleFor(t => t.CompanyId).NotEmpty().WithErrorCode("INVALID_COMPANY_ID");
            RuleFor(t => t.Name).MustAsync(NameMustBeUnique).WithErrorCode("TOKEN_ALREADY_EXISTS");
            RuleFor(t => t.Symbol).MustAsync(SymbolMustBeUnique).WithErrorCode("TOKEN_ALREADY_EXISTS");
            RuleFor(t => t.OwnerAddress).NotEmpty();
            RuleFor(t => t.ContractVersion).NotEmpty();
        }

        private async Task<bool> NameMustBeUnique(string name, CancellationToken t)
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var dbContext = scope.Resolve<IWalletDbContext>();
                var normalizedName = name.ToUpperInvariant();
                return (await dbContext.Tokens.AsNoTracking()
                    .CountAsync(t => t.NameNormalized == normalizedName, cancellationToken: t)) == 0;
            }
        }
        
        private async Task<bool> SymbolMustBeUnique(string symbol, CancellationToken t)
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var dbContext = scope.Resolve<IWalletDbContext>();
                var symbolNormalized = symbol.ToUpperInvariant();
                return (await dbContext.Tokens.AsNoTracking()
                    .CountAsync(t => t.SymbolNormalized == symbolNormalized, cancellationToken: t)) == 0;
            }
        }
    }
}