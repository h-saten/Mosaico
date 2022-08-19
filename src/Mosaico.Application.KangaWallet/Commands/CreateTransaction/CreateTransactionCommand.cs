using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.KangaWallet.Commands.CreateTransaction
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class CreateTransactionCommand : IRequest<KangaBuyResponseDto>
    {
        public string UserId { get; set; }
        public string PaymentCurrency { get; set; }
        public string ProjectId { get; set; }
        public decimal TokenAmount { get; set; }
        public decimal CurrencyAmount { get; set; }
        public string RefCode { get; set; }
    }
}