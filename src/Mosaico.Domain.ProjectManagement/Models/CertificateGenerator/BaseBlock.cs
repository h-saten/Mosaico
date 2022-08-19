namespace Mosaico.Domain.ProjectManagement.Models.CertificateGenerator
{
    public class BaseBlock : ICertificateBlock
    {
        public bool Enabled { get; set; }
        public decimal MarginTop { get; set; }
        public decimal MarginLeft { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        
        public bool IsValid()
        {
            return MarginTop > 0 && MarginLeft > 0 && Width > 0 && Height > 0;
        }
    }
}