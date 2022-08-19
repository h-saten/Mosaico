using System;
using Mosaico.Domain.ProjectManagement.Models.CertificateGenerator;

namespace Mosaico.Application.ProjectManagement.Services
{
    public class CertificateConfigurationMapper
    {
        public CertificateTheme Map(
            string tokenTicker,
            string name, 
            string tokensAmount, 
            string date,
            string logoUrl,
            CertificateCode certificateCode,
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
                    FontSize = certificateParams?.InvestorName.FontSizePx.ToString(),
                    MarginLeft = certificateParams?.InvestorName.MarginLeft.ToString(),
                    MarginTop = certificateParams?.InvestorName.MarginTop.ToString(),
                    LineHeight = certificateParams?.InvestorName.FontSizePx.ToString(),
                    PaddingTop =
                        ((certificateParams?.InvestorName.Height - certificateParams?.InvestorName.FontSizePx) / 2)
                        .ToString(),
                    FontWeight =
                        certificateParams?.InvestorName.TextBold != null && certificateParams.InvestorName.TextBold
                            ? "bold"
                            : "normal",
                    TextAlign = certificateParams?.InvestorName.TextAlign?.ToLower() ?? "center"
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
                    FontSize = certificateParams?.TokensAmount.FontSizePx.ToString(),
                    MarginLeft = certificateParams?.TokensAmount.MarginLeft.ToString(),
                    MarginTop = certificateParams?.TokensAmount.MarginTop.ToString(),
                    LineHeight = certificateParams?.TokensAmount.FontSizePx.ToString(),
                    PaddingTop =
                        ((certificateParams?.TokensAmount.Height - certificateParams?.TokensAmount.FontSizePx) / 2)
                        .ToString(),
                    FontWeight =
                        certificateParams?.TokensAmount.TextBold != null && certificateParams.TokensAmount.TextBold
                            ? "bold"
                            : "normal",
                    TextAlign = certificateParams?.InvestorName.TextAlign?.ToLower() ?? "center"
                },
                ShowCode = certificateParams != null && certificateParams.Code.Enabled,
                CodeConfiguration = new ElementConfiguration
                {
                    Value = certificateCode.ToString(),
                    Height = certificateParams?.Code.Height.ToString(),
                    Width = certificateParams?.Code.Width.ToString(),
                    FontColor = certificateParams?.Code.FontColor,
                    FontSize = certificateParams?.Code.FontSizePx.ToString(),
                    MarginLeft = certificateParams?.Code.MarginLeft.ToString(),
                    MarginTop = certificateParams?.Code.MarginTop.ToString(),
                    LineHeight = certificateParams?.Code.FontSizePx.ToString(),
                    PaddingTop = ((certificateParams?.Code.Height - certificateParams?.Code.FontSizePx) / 2).ToString(),
                    FontWeight = certificateParams?.Code.TextBold != null && certificateParams.Code.TextBold
                        ? "bold"
                        : "normal",
                    TextAlign = certificateParams?.InvestorName.TextAlign?.ToLower() ?? "center"
                },
                ShowDate = certificateParams != null && certificateParams.Date.Enabled,
                DateConfiguration = new ElementConfiguration
                {
                    Value = date,
                    Height = certificateParams?.Date.Height.ToString(),
                    Width = certificateParams?.Date.Width.ToString(),
                    FontColor = certificateParams?.Date.FontColor,
                    FontSize = certificateParams?.Date.FontSizePx.ToString(),
                    MarginLeft = certificateParams?.Date.MarginLeft.ToString(),
                    MarginTop = certificateParams?.Date.MarginTop.ToString(),
                    LineHeight = certificateParams?.Date.FontSizePx.ToString(),
                    PaddingTop = ((certificateParams?.Date.Height - certificateParams?.Date.FontSizePx) / 2).ToString(),
                    FontWeight = certificateParams?.Date.TextBold != null && certificateParams.Date.TextBold
                        ? "bold"
                        : "normal",
                    TextAlign = certificateParams?.InvestorName.TextAlign?.ToLower() ?? "center"
                },
                ShowLogo = certificateParams.LogoBlock.Enabled,
                LogoConfiguration = new ElementConfiguration
                {
                    Value = logoUrl,
                    Height = certificateParams?.LogoBlock.Height.ToString(),
                    Width = certificateParams?.LogoBlock.Width.ToString(),
                    MarginLeft = certificateParams?.LogoBlock.MarginLeft.ToString(),
                    MarginTop = certificateParams?.LogoBlock.MarginTop.ToString(),
                    BorderRadius = certificateParams.LogoBlock.IsRounded ? "50" : "0",
                }
            };

            return config;
        }
    }
}