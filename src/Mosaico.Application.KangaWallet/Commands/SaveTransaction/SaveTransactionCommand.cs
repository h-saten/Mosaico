using MediatR;

namespace Mosaico.Application.KangaWallet.Commands.SaveTransaction
{
    public class SaveTransactionCommand : IRequest
    {
        public string TransactionId { get; set; }
    }
}
