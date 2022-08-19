using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Base;
using Mosaico.Base.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Integration.Email.Abstraction;
using Serilog;

namespace Mosaico.Application.Identity.Services
{
    public class IdentityEmailService : IIdentityEmailService
    {
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateEngine _templateEngine;
        private readonly IResourceManager _resourceManager;
        private readonly IIndex<string, string> _urls;

        public IdentityEmailService(IEmailSender emailSender, ITemplateEngine templateEngine, IResourceManager resourceManager, IIndex<string, string> urls, ILogger logger = null)
        {
            _emailSender = emailSender;
            _templateEngine = templateEngine;
            _resourceManager = resourceManager;
            _urls = urls;
            _logger = logger;
        }

        public async Task SendForgotPasswordEmailAsync(ApplicationUser user, string callbackUrl, string language = Base.Constants.Languages.English)
        {
            var template =
                await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                    "PasswordReset", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "PasswordReset", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var email = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "ResetUrl", callbackUrl } }),
                Recipients = new List<string> { user.Email },
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

            await _emailSender.SendAsync(email);
        }
        public async Task SendPasswordChangeConfirmationCodeEmailAsync(string userEmail, string code, string language = Base.Constants.Languages.English)
        {
            var template =
                await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                    "PasswordChangeConfirmationCode", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "PasswordChangeConfirmationCode", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo = await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);
            var email = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "code", code} }),
                Recipients = new List<string> { userEmail },
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

            await _emailSender.SendAsync(email);
        }
        
        public async Task SendEmailChangeEmailAsync(string callbackUrl, string email, string language = Base.Constants.Languages.English)
        {
            var template =
                await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                    "ChangeEmail", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "ChangeEmail", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var Newemail = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "ChangeEmailUrl", callbackUrl } }),
                Recipients = new List<string> { email },
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

        public async Task SendEmailChangedNotificationAsync(string OldEmail, string callbackUrl, string language = Base.Constants.Languages.English)
        {
            var template =
                await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                    "EmailChangeConfirmation", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "EmailChangeConfirmation", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var email = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "ReportUrl", callbackUrl } }),
                Recipients = new List<string> { OldEmail },
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
            await _emailSender.SendAsync(email);
        }

        public async Task SendPasswordChangedNotificationAsync(string email, string callbackUrl, string language = Base.Constants.Languages.English)
        {
            var template =
               await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                   "PasswordChangeConfirmation", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "PasswordChangeConfirmation", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var newEmail = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "ReportUrl", callbackUrl } }),
                Recipients = new List<string> { email },
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
            await _emailSender.SendAsync(newEmail);
        }

        public async Task SendPhoneNumberChangedNotificationAsync(string email, string callbackUrl, string language = "en")
        {
            var template =
               await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                   "PhoneNumberChangeConfirmation", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "PhoneNumberChangeConfirmation", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var newEmail = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "ReportUrl", callbackUrl } }),
                Recipients = new List<string> { email },
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
            await _emailSender.SendAsync(newEmail);
        }

        public async Task SendEmailConfirmationEmailAsync(ApplicationUser user, string callbackUrl, string language = Base.Constants.Languages.English)
        {
            var template =
                await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                    "EmailConfirmation", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "EmailConfirmation", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var email = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "ActivationUrl", callbackUrl } }),
                Recipients = new List<string> { user.Email },
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
            await _emailSender.SendAsync(email);
        }

        public async Task SendExternalUserConfirmationEmailAsync(ApplicationUser user, string callbackUrl, string language = Base.Constants.Languages.English)
        {
            var template =
                await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                    "EmailConfirmationExternalUser", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "EmailConfirmation", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var email = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "ActivationUrl", callbackUrl } }),
                Recipients = new List<string> { user.Email },
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
            await _emailSender.SendAsync(email);
        }
        
        public async Task SendDeviceAuthorizationCode(string email, string code, string language = Base.Constants.Languages.English)
        {
            var template =
                await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                    "DeviceAuthorization", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "DeviceAuthorization", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var emailMessage = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "Code", code } }),
                Recipients = new List<string> { email },
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
            await _emailSender.SendAsync(emailMessage);
            // Console.WriteLine("********************************");
            // Console.WriteLine($"Email code: {code}");
            // Console.WriteLine("********************************");
            // return Task.CompletedTask;
        }

        public async Task SendKycCompletedSuccessfullyAsync(string email, string language = Base.Constants.Languages.English)
        {
            var baseUri = "/";
            if (_urls.TryGetValue(Constants.UrlKeys.BaseUri, out var fetchedBaseUri))
                baseUri = fetchedBaseUri;
            
            var template =
                await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.EmailTemplates,
                    "KycVerificationSucceeded", language);
            var subjectTemplate =
                await _resourceManager.GetResourceNameAsync(Core.ResourceManager.Constants.Categories.EmailSubjects,
                    "KycVerificationSucceeded", language);
            var logoFileName = "logo_mosaico.png";
            var mosaicoLogo =
                await _resourceManager.GetResourceAsync(Core.ResourceManager.Constants.Categories.Images, logoFileName);

            var emailMessage = new Email
            {
                Html = _templateEngine.Build(template, new Dictionary<string, string> { { "MosaicoUrl", baseUri } }),
                Recipients = new List<string> { email },
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
            await _emailSender.SendAsync(emailMessage);
        }
    }
}