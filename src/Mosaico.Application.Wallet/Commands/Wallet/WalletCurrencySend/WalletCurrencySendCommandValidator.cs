using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Wallet.WalletCurrencySend
{
    public class WalletSendCurrencyCommandValidator : AbstractValidator<WalletSendCurrencyCommand>
    {
        public WalletSendCurrencyCommandValidator()
        {
            RuleFor(t => t.PaymentCurrencyId).NotEmpty();
            RuleFor(c => c.Amount).GreaterThan((decimal)0.1).WithErrorCode("INVALID_SEND_AMOUNT");
            RuleFor(c => c.Network).Must(n => Blockchain.Base.Constants.BlockchainNetworks.All.Contains(n));
            RuleFor(t => t.WalletAddress).NotEmpty();
            RuleFor(t => t.Address).NotEmpty();
        }
    }
}