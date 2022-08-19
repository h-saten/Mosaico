namespace Mosaico.KYC.Passbase.Configurations
{
    public class PassbaseConfig
    {
        public const string SectionName = "Passbase";
        
        public string ApiSecret { get; set; }
        public string Url { get; set; }
    }
}