
// namespace Mosaico.PDF.Base.Generator
// {
//     public class InvestorCertificateGenerator : PdfGenerator, IInvestorCertificateGenerator
//     {
//         private readonly IGeneratePdf _generatePdf;
//         private const string BaseLanguage = "en";
//         
//         public InvestorCertificateGenerator(IGeneratePdf generatePdf)
//         {
//             _generatePdf = generatePdf;
//         }
        
        // public async Task<byte[]> GeneratePdf<TConfiguration>(Action<TConfiguration> configuration) where TConfiguration : class, new()
        // {
        //     var config = new TConfiguration();
        //     configuration.Invoke(config);
        //     
        //     return await GenerateFromConfiguration(
        //         config.TokenTicker,
        //         config.Name,
        //         config.TokensAmount,
        //         config.Date,
        //         config.LogoUrl,
        //         config.CertificateCode,
        //         config.BackgroundUrl,
        //         config.CertificateParams);
        // }

        /*
        public async Task<byte[]> GeneratePdf(Action<CertificateGeneratorConfiguration> configuration)
        {
            var config = new CertificateGeneratorConfiguration();
            configuration.Invoke(config);
            
            return await GenerateFromConfiguration(
                config.TokenTicker,
                config.Name,
                config.TokensAmount,
                config.Date,
                config.LogoUrl,
                config.CertificateCode,
                config.BackgroundUrl,
                config.CertificateParams);
        }

        public async Task<byte[]> PdfToImage(Action<CertificateGeneratorConfiguration> configuration)
        {
            var config = new CertificateGeneratorConfiguration();
            configuration.Invoke(config);
            
            byte[] pdfBytes = await GeneratePdf(configuration);
            
            var certificatePath = PdfTemporaryPath();
            
            await File.WriteAllBytesAsync(certificatePath, pdfBytes);

            return await PdfToImageAsBytes(certificatePath);
        }
        
        public async Task<string> GeneratePdfPath(Action<CertificateGeneratorConfiguration> configuration)
        {
            var config = new CertificateGeneratorConfiguration();
            configuration.Invoke(config);
            
            byte[] pdfBytes = await GeneratePdf(configuration);
            
            var tempPath = Path.GetTempPath();

            var certificatePath = $"{tempPath}{Guid.NewGuid()}.pdf";

            await File.WriteAllBytesAsync(certificatePath, pdfBytes);
            
            return certificatePath;
        }
        
        public async Task<byte[]> PdfBytesToImageBytes(byte[] pdfBytes)
        {
            var certificatePath = PdfTemporaryPath();
            
            await File.WriteAllBytesAsync(certificatePath, pdfBytes);

            return await PdfToImageAsBytes(certificatePath);
        }
        
        private async Task<byte[]> GenerateFromConfiguration(
            string tokenTicker,
            string name, 
            string tokensAmount, 
            string date,
            string logoUrl,
            string certificateCode,
            string backgroundUrl,
            CertificateConfiguration certificateParams)
        {
            var config = new CertificateTheme
            {
                BackgroundUrl =
                    backgroundUrl,
                LogoUrl = logoUrl,
                BasePath = AppContext.BaseDirectory,
                ShowName = certificateParams != null && certificateParams.InvestorName.Enabled,
                NameConfiguration = new ElementConfiguration
                {
                    Value = name,
                    Height = certificateParams?.InvestorName.Height.ToString(),
                    Width = certificateParams?.InvestorName.Width.ToString(),
                    FontColor = certificateParams?.InvestorName.FontColor,
                    FontSize = certificateParams?.InvestorName.FontSize.ToString(),
                    MarginLeft = certificateParams?.InvestorName.MarginLeft.ToString(),
                    MarginTop = certificateParams?.InvestorName.MarginTop.ToString(),
                    LineHeight = certificateParams?.InvestorName.FontSize.ToString(),
                    PaddingTop =
                        ((certificateParams?.InvestorName.Height - certificateParams?.InvestorName.FontSize) / 2)
                        .ToString(),
                    FontWeight =
                        certificateParams?.InvestorName.TextBold != null && certificateParams.InvestorName.TextBold
                            ? "bold"
                            : "normal",
                    TextAlign =
                        certificateParams?.InvestorName.TextAlign != null &&
                        certificateParams?.InvestorName.TextAlign != 0
                            ? Enum.GetName(typeof(CertificateBlockTextAlign), certificateParams.InvestorName.TextAlign)
                                .ToLower()
                            : "center"
                },
                ShowTokensAmount = certificateParams != null && certificateParams.TokensAmount.Enabled,
                TokensAmountConfiguration = new ElementConfiguration
                {
                    Value = certificateParams.TokensAmount.AttachTicker
                        ? tokensAmount + " " + tokenTicker
                        : tokensAmount,
                    Height = certificateParams?.TokensAmount.Height.ToString(),
                    Width = certificateParams?.TokensAmount.Width.ToString(),
                    FontColor = certificateParams?.TokensAmount.FontColor,
                    FontSize = certificateParams?.TokensAmount.FontSize.ToString(),
                    MarginLeft = certificateParams?.TokensAmount.MarginLeft.ToString(),
                    MarginTop = certificateParams?.TokensAmount.MarginTop.ToString(),
                    LineHeight = certificateParams?.TokensAmount.FontSize.ToString(),
                    PaddingTop =
                        ((certificateParams?.TokensAmount.Height - certificateParams?.TokensAmount.FontSize) / 2)
                        .ToString(),
                    FontWeight =
                        certificateParams?.TokensAmount.TextBold != null && certificateParams.TokensAmount.TextBold
                            ? "bold"
                            : "normal",
                    TextAlign =
                        certificateParams?.TokensAmount.TextAlign != null &&
                        certificateParams?.TokensAmount.TextAlign != 0
                            ? Enum.GetName(typeof(CertificateBlockTextAlign), certificateParams.TokensAmount.TextAlign)
                                .ToLower()
                            : "center"
                },
                ShowCode = certificateParams != null && certificateParams.Code.Enabled,
                CodeConfiguration = new ElementConfiguration
                {
                    Value = certificateCode,
                    Height = certificateParams?.Code.Height.ToString(),
                    Width = certificateParams?.Code.Width.ToString(),
                    FontColor = certificateParams?.Code.FontColor,
                    FontSize = certificateParams?.Code.FontSize.ToString(),
                    MarginLeft = certificateParams?.Code.MarginLeft.ToString(),
                    MarginTop = certificateParams?.Code.MarginTop.ToString(),
                    LineHeight = certificateParams?.Code.FontSize.ToString(),
                    PaddingTop = ((certificateParams?.Code.Height - certificateParams?.Code.FontSize) / 2).ToString(),
                    FontWeight = certificateParams?.Code.TextBold != null && certificateParams.Code.TextBold
                        ? "bold"
                        : "normal",
                    TextAlign = certificateParams?.Code.TextAlign != null && certificateParams?.Code.TextAlign != 0
                        ? Enum.GetName(typeof(CertificateBlockTextAlign), certificateParams.Code.TextAlign).ToLower()
                        : "center"
                },
                ShowDate = certificateParams != null && certificateParams.Date.Enabled,
                DateConfiguration = new ElementConfiguration
                {
                    Value = date,
                    Height = certificateParams?.Date.Height.ToString(),
                    Width = certificateParams?.Date.Width.ToString(),
                    FontColor = certificateParams?.Date.FontColor,
                    FontSize = certificateParams?.Date.FontSize.ToString(),
                    MarginLeft = certificateParams?.Date.MarginLeft.ToString(),
                    MarginTop = certificateParams?.Date.MarginTop.ToString(),
                    LineHeight = certificateParams?.Date.FontSize.ToString(),
                    PaddingTop = ((certificateParams?.Date.Height - certificateParams?.Date.FontSize) / 2).ToString(),
                    FontWeight = certificateParams?.Date.TextBold != null && certificateParams.Date.TextBold
                        ? "bold"
                        : "normal",
                    TextAlign = certificateParams?.Date.TextAlign != null && certificateParams?.Date.TextAlign != 0
                        ? Enum.GetName(typeof(CertificateBlockTextAlign), certificateParams.Date.TextAlign).ToLower()
                        : "center"
                },
                ShowLogo = certificateParams.LogoBlock.Enabled,
                LogoConfiguration = new ElementConfiguration
                {
                    Value = logoUrl,
                    Height = certificateParams?.LogoBlock.Height.ToString(),
                    Width = certificateParams?.LogoBlock.Width.ToString(),
                    MarginLeft = certificateParams?.LogoBlock.MarginLeft.ToString(),
                    MarginTop = certificateParams?.LogoBlock.MarginTop.ToString(),
                    BorderRadius = certificateParams.LogoBlock.Rounded ? "50" : "0",
                }
            };
            
            return await GeneratePdfForCustomConfigurationAsBytes(config);
        }
        
        private async Task<byte[]> GeneratePdfForCustomConfigurationAsBytes(CertificateTheme themeConfiguration)
        {
            var templateUrl = @"./MustacheTemplates/Certificate/GeneralInvestorCertificateForEditor.html";
            var basePath = AppContext.BaseDirectory;
            
            var backgroundUrl = themeConfiguration.BackgroundUrl;
            var logoUrl = themeConfiguration.LogoUrl;
            var model = themeConfiguration;

            if (backgroundUrl != null)
            {
              Uri backgroundUri = new Uri(backgroundUrl);
              var backgroundFileName = Path.GetFileName(backgroundUri.LocalPath);
              var backgroundPath = $"{basePath}Images/Certificate/{backgroundFileName}";
  
              // BACKGROUND
              var dir = $"{basePath}Images/Certificate";
              if (!Directory.Exists(dir))
              {
                  Directory.CreateDirectory(dir);
              }
              using WebClient myWebClient = new WebClient();
              myWebClient.DownloadFile(backgroundUrl, backgroundPath);
              model.BackgroundUrl = backgroundPath;  
            }

            if (logoUrl != null)
            {
                Uri logoUri = new Uri(logoUrl);
                var logoFileName = Path.GetFileName(logoUri.LocalPath);
                var logoPath = $"{basePath}Images/Certificate/{logoFileName}";
            
                // LOGO
                var dir2 = $"{basePath}Images/Certificate";
                if (!Directory.Exists(dir2))
                {
                    Directory.CreateDirectory(dir2);
                }
                using WebClient myWebClient2 = new WebClient();
                myWebClient2.DownloadFile(logoUrl, logoPath);
                model.LogoUrl = logoPath; 
            }

            var certificateContent = await ProcessTemplate(model, templateUrl);
            
            var options = new CustomConvertOptions
            {
                PageOrientation = Orientation.Landscape,
                PageMargins = new Margins(0 ,0 ,0 ,0),
                KeepRelative = true
            };
            
            _generatePdf.SetConvertOptions(options);
            
            return _generatePdf.GetPDF(certificateContent);
        }
        
        */
//     }
// }