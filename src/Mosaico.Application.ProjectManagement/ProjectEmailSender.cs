using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Base;
using Mosaico.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Integration.Email.Abstraction;

namespace Mosaico.Application.ProjectManagement
{
    public class ProjectEmailSender : IProjectEmailSender
    {
        private readonly IEmailSender _emailSender;
        private readonly ITemplateEngine _templateEngine;
        private readonly IResourceManager _resourceManager;

        public ProjectEmailSender(IEmailSender emailSender, ITemplateEngine templateEngine, IResourceManager resourceManager)
        {
            _emailSender = emailSender;
            _templateEngine = templateEngine;
            _resourceManager = resourceManager;
        }

        public async Task SendInvestorInvitationsAsync(Project prj, List<string> recipients)
        {
            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates, "InvestorInvitation");
            var subjectTemplate = await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects, "InvestorInvitation");
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var payload = new Dictionary<string, string> {{"projectTitle", prj.Title}};
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
        
        public async Task SendInvitationOnProjectMemberAddedAsync(Project prj, string callbackUrl, List<string> email, string language = Base.Constants.Languages.English)
        {
            var template =
                await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                    "ProjectMemberInvitation", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "ProjectMemberInvitation", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var Newemail = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "url", callbackUrl },{ "title", prj.Title} }),
                Recipients = email,
                Subject = _templateEngine.Build(subjectTemplate, null),
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

            await _emailSender.SendAsync(Newemail);

        }
    }
}