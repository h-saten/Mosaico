namespace Mosaico.Application.KangaWallet.Commands.CreateTransaction
{
    public class KangaBuyResponseDto
    {
        public string TransactionId { get; set; }
        public string RedirectUrl { get; set; }
        public bool KangaAccountCreated { get; set; }
    }
}