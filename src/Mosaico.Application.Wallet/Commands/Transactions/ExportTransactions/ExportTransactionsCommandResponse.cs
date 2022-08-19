namespace Mosaico.Application.Wallet.Commands.Transactions.ExportTransactions
{
    public class ExportTransactionsCommandResponse
    {
        public int Count { get; set; }
        public byte[] File { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
    }
}