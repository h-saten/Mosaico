using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Base;
using Mosaico.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Integration.Email.Abstraction;

namespace Mosaico.Application.ProjectManagement.Services
{
    public class AffiliationEmailService : IAffiliationEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly ITemplateEngine _templateEngine;
        private readonly IResourceManager _resourceManager;

        public AffiliationEmailService(IEmailSender emailSender, ITemplateEngine templateEngine, IResourceManager resourceManager)
        {
            _emailSender = emailSender;
            _templateEngine = templateEngine;
            _resourceManager = resourceManager;
        }
        
        public async Task SendInvestorAffiliationInvitationAsync(Project prj, string regulationLink, List<string> recipients, string language = Base.Constants.Languages.English)
        {
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "InvestorAffiliationInvitation", language);
            var subjectTemplate = await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects, "InvestorAffiliationInvitation", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var payload = new Dictionary<string, string>
            {
                {"projectTitle", prj.Title},
                {"regulationUrl", regulationLink}
            };
            
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