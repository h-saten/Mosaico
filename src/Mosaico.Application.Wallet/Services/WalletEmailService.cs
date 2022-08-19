using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base;
using Mosaico.Base.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.SDK.ProjectManagement.Models;

namespace Mosaico.Application.Wallet.Services
{
    public class WalletEmailService : IWalletEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly ITemplateEngine _templateEngine;
        private readonly IResourceManager _resourceManager;
        private readonly IIndex<string, string> _urls;

        public WalletEmailService(IEmailSender emailSender, ITemplateEngine templateEngine, IResourceManager resourceManager, IIndex<string, string> urls)
        {
            _emailSender = emailSender;
            _templateEngine = templateEngine;
            _resourceManager = resourceManager;
            _urls = urls;
        }
        
        public async Task SendStakingDeactivatedAsync(string tokenSymbol, string recipient, decimal amount, string language = Base.Constants.Languages.English)
        {
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "StakingDeactivated", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "StakingDeactivated", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);
            
            var baseUri = "/";
            if (_urls.TryGetValue("BaseUri", out var fetchedBaseUri))
                baseUri = fetchedBaseUri;
            
            var payload = new Dictionary<string, string>
            {
                {"TokenTicker", tokenSymbol},
                {"TokenAmount", amount.ToString("F") ?? "0.00"},
                {"MosaicoWalletUrl", Flurl.Url.Combine(baseUri, "/wallet/dashboard")}
            };
            var email = new Email
            {
                Html = _templateEngine.Build(template, payload),
                Recipients = new List<string>{recipient},
                Subject = _templateEngine.Build(subjectTemplate, payload),
                Attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        Content = mosaicoLogo,
                        IsInline = true,
                        FileName = logoFileName,
                        ContentType = MimeTypes.GetMimeType(logoFileName)
                    }
                }
            };
                    
            await _emailSender.SendAsync(email);
        }
        
        public async Task SendStakingActivatedAsync(string tokenSymbol, string recipient, decimal amount, decimal apr, string language = Base.Constants.Languages.English)
        {
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "StakingActivated", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "StakingActivated", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);
            
            var baseUri = "/";
            if (_urls.TryGetValue("BaseUri", out var fetchedBaseUri))
                baseUri = fetchedBaseUri;
            
            var payload = new Dictionary<string, string>
            {
                {"TokenTicker", tokenSymbol},
                {"TokenAmount", amount.ToString("F") ?? "0.00"},
                {"APR", apr.ToString("F") ?? "0.00"},
                {"MosaicoWalletUrl", Flurl.Url.Combine(baseUri, "/wallet/dashboard")}
            };
            var email = new Email
            {
                Html = _templateEngine.Build(template, payload),
                Recipients = new List<string>{recipient},
                Subject = _templateEngine.Build(subjectTemplate, payload),
                Attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        Content = mosaicoLogo,
                        IsInline = true,
                        FileName = logoFileName,
                        ContentType = MimeTypes.GetMimeType(logoFileName)
                    }
                }
            };
                    
            await _emailSender.SendAsync(email);
        }
        
        public async Task SendStakingRewardPaid(Token token, string recipient, decimal amount, PaymentCurrency currency, string language = Base.Constants.Languages.English)
        {
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "StakingRewardPaid", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "StakingRewardPaid", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);
            
            var baseUri = "/";
            if (_urls.TryGetValue("BaseUri", out var fetchedBaseUri))
                baseUri = fetchedBaseUri;
            
            var payload = new Dictionary<string, string>
            {
                {"TokenTicker", token.Symbol},
                {"CurrencyAmount", amount.ToString("F") ?? "0.00"},
                {"SelectedCurrencyName", currency.Ticker},
                {"MosaicoWalletUrl", Flurl.Url.Combine(baseUri, "/wallet/dashboard")}
            };
            var email = new Email
            {
                Html = _templateEngine.Build(template, payload),
                Recipients = new List<string>{recipient},
                Subject = _templateEngine.Build(subjectTemplate, payload),
                Attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        Content = mosaicoLogo,
                        IsInline = true,
                        FileName = logoFileName,
                        ContentType = MimeTypes.GetMimeType(logoFileName)
                    }
                }
            };
                    
            await _emailSender.SendAsync(email);
        }
        
        public async Task SendStakingRewardPaid(PaymentCurrency token, string recipient, decimal amount, PaymentCurrency currency, string language = Base.Constants.Languages.English)
        {
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "StakingRewardPaid", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "StakingRewardPaid", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);
            
            var baseUri = "/";
            if (_urls.TryGetValue("BaseUri", out var fetchedBaseUri))
                baseUri = fetchedBaseUri;
            
            var payload = new Dictionary<string, string>
            {
                {"TokenTicker", token.Ticker},
                {"CurrencyAmount", amount.ToString("F") ?? "0.00"},
                {"SelectedCurrencyName", currency.Ticker},
                {"MosaicoWalletUrl", Flurl.Url.Combine(baseUri, "/wallet/dashboard")}
            };
            var email = new Email
            {
                Html = _templateEngine.Build(template, payload),
                Recipients = new List<string>{recipient},
                Subject = _templateEngine.Build(subjectTemplate, payload),
                Attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        Content = mosaicoLogo,
                        IsInline = true,
                        FileName = logoFileName,
                        ContentType = MimeTypes.GetMimeType(logoFileName)
                    }
                }
            };
                    
            await _emailSender.SendAsync(email);
        }
        
        public async Task SendTransactionFinishedAsync(Transaction transaction, MosaicoProject project, Token token, string recipient, string language = Base.Constants.Languages.English)
        {
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "TransactionFinishedEmail", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "TransactionFinishedEmail", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);
            
            var baseUri = "/";
            if (_urls.TryGetValue("BaseUri", out var fetchedBaseUri))
                baseUri = fetchedBaseUri;
            
            var payload = new Dictionary<string, string>
            {
                {"ProjectName", project.Title},
                {"TokenTicker", token.Symbol},
                {"CurrencyAmount", transaction.PayedAmount?.ToString("F") ?? "0.00"},
                {"SelectedCurrencyName", transaction.Currency},
                {"TokensAmount", transaction.TokenAmount?.ToString("F") ?? "0.00"},
                {"MosaicoWalletUrl", Flurl.Url.Combine(baseUri, "/wallet")}
            };
            var email = new Email
            {
                Html = _templateEngine.Build(template, payload),
                Recipients = new List<string>{recipient},
                Subject = _templateEngine.Build(subjectTemplate, payload),
                Attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        Content = mosaicoLogo,
                        IsInline = true,
                        FileName = logoFileName,
                        ContentType = MimeTypes.GetMimeType(logoFileName)
                    }
                }
            };
                    
            await _emailSender.SendAsync(email);
        }
        
        public async Task SendBankTransferDetailsAsync(Transaction transaction, MosaicoProject project, Token token, ProjectBankTransferTitle transferTitle, string recipient, string language = Base.Constants.Languages.English)
        {
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "BankTransferDetails", language);
            var subjectTemplate = await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects, "BankTransferDetails", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var payload = new Dictionary<string, string>
            {
                {"account", transferTitle.ProjectBankPaymentDetails.Account},
                {"title", transferTitle.Reference},
                {"recipient", transferTitle.ProjectBankPaymentDetails.AccountAddress},
                {"bank", transferTitle.ProjectBankPaymentDetails.BankName},
                {"swift", transferTitle.ProjectBankPaymentDetails.Swift},
                {"amount", transferTitle.FiatAmount.ToString("F")},
                {"currency", transferTitle.Currency},
                {"tokenAmount", transferTitle.TokenAmount.ToString("F")},
                {"tokenSymbol", token.Symbol},
                {"projectName", project.Title}
            };
            var email = new Email
            {
                Html = _templateEngine.Build(template, payload),
                Recipients = new List<string>{recipient},
                Subject = _templateEngine.Build(subjectTemplate, payload),
                Attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        Content = mosaicoLogo,
                        IsInline = true,
                        FileName = logoFileName,
                        ContentType = MimeTypes.GetMimeType(logoFileName)
                    }
                }
            };
                    
            await _emailSender.SendAsync(email);
        }
    }
}