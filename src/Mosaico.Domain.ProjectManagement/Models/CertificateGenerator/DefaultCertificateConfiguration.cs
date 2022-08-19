namespace Mosaico.Domain.ProjectManagement.Models.CertificateGenerator
{
    public class DefaultCertificateConfiguration : CertificateConfiguration
    {
        private const string DefaultCertificateBackgroundUrl = "https://files.mosaico.ai/app/certificates/general_certificate_bg.jpg";
        private const string DefaultCertificateBackgroundPath = "https://files.mosaico.ai/app/certificates/general_certificate_bg_{0}.jpg";

        public DefaultCertificateConfiguration()
        {
            Code = new BaseTextBlock
            {
                Enabled = true,
                Height = 80,
                Width = 400,
                FontColor = "#0080FD",
                FontSizePx = 19,
                MarginLeft = 564,
                MarginTop = 982,
                TextAlign = "CENTER",
            };
            LogoBlock = new LogoBlock
            {
                Enabled = true,
                Height = 194,
                Width = 194,
                MarginLeft = 667,
                MarginTop = 406,
                IsRounded = true,
            };
            Date = new BaseTextBlock
            {
                Enabled = false,
                FontSizePx = 19,
                FontColor = "#000",
                TextBold = false,
                TextAlign = "CENTER",
                MarginTop = 893,
                MarginLeft = 564,
                Width = 400,
                Height = 80
            };
            TokensAmount = new TokensAmountBaseBlock
            {
                Enabled = true,
                AttachTicker = true,
                FontSizePx = 70,
                FontColor = "#FF9401",
                TextBold = true,
                TextAlign = "CENTER",
                MarginTop = 772,
                MarginLeft = 400,
                Height = 99,
                Width = 728
            };
            InvestorName = new BaseTextBlock
            {
                Enabled = true,
                FontSizePx = 36,
                FontColor = "#676767",
                TextBold = true,
                TextAlign = "CENTER",
                MarginTop = 628,
                MarginLeft = 400,
                Width = 728,
                Height = 80
            };
        }

        public static string GetDefaultCertificateBackgroundUrl(string language = "en")
        {
            return string.Format(DefaultCertificateBackgroundPath, language);
        }
    }
}