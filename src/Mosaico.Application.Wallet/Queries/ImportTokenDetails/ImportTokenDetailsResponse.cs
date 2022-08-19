namespace Mosaico.Application.Wallet.Queries.ImportTokenDetails
{
    public class ImportTokenDetailsResponse
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Decimals { get; set; }
        public decimal TotalSupply { get; set; }
        public bool CanImport { get; set; }
        public bool Burnable { get; set; }
        public bool Mintable { get; set; }
    }
}