namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class TokenDTO
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public long TotalSupply { get; set; }
        public string Network { get; set; }
        public bool IsMintable { get; set; }
        public bool IsBurnable { get; set; }
        public string Type { get; set; }
        public string LogoUrl { get; set; }
    }
}