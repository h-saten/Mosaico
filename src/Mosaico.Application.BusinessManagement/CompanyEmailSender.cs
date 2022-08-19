using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Application.BusinessManagement.Abstractions;
using Mosaico.Base;
using Mosaico.Base.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Integration.Email.Abstraction;

namespace Mosaico.Application.BusinessManagement
{
    public class CompanyEmailSender : ICompanyEmailSender
    {
        private readonly IEmailSender _emailSender;
        private readonly ITemplateEngine _templateEngine;
        private readonly IResourceManager _resourceManager;

        public CompanyEmailSender(IEmailSender emailSender, ITemplateEngine templateEngine, IResourceManager resourceManager)
        {
            _emailSender = emailSender;
            _templateEngine = templateEngine;
            _resourceManager = resourceManager;
        }

        public async Task SendEmailRequestsOnCompanyVerificationAsync(string title, List<string> recipients, string callbackUrl, string language = Base.Constants.Languages.English)
        {
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "CompanyVerificationRequest", language);
            var subjectTemplate = await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects, "CompanyVerificationRequest", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var payload = new Dictionary<string, string> { { "name", title }, { "url", callbackUrl } };
            var email = new Email
            {
                Html = _templateEngine.Build(template, payload),
                Recipients = recipients,
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