namespace Mosaico.Base.Configurations
{
    public class RecaptchaConfiguration
    {
        public static string SectionName = "Recaptcha";
        
        public string ApiSecret { get; set; }
        public string Url { get; set; }
    }
}