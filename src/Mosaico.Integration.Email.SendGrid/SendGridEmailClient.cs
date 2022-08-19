using FluentValidation;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Integration.Email.SendGridEmail.Configurations;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Integration.Email.SendGridEmail
{
    public class SendGridEmailClient : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly SendGridEmailConfig _sendGridConfig;
        private readonly IValidator<Abstraction.Email> _emailValidator;
        public SendGridEmailClient(SendGridEmailConfig sendGridConfig, IValidator<Abstraction.Email> emailValidator = null, ILogger logger = null)
        {
            _sendGridConfig = sendGridConfig;
            _emailValidator = emailValidator;
            _logger = logger;
        }

        public async Task<EmailSentResult> SendAsync(Abstraction.Email email, CancellationToken t = new())
        {
            try
            {
                var client = new SendGridClient(_sendGridConfig.AppKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_sendGridConfig.FromEmail, _sendGridConfig.DisplayName),
                    Subject = email.Subject,
                    HtmlContent = email.Html
                };
                if (email.Attachments != null)
                {
                    //List<Attachment> attachmentList = new List<Attachment>();
                    foreach (var attachment in email.Attachments)
                    {
                        Attachment attachment2 = new Attachment();
                        attachment2.ContentId = Guid.NewGuid().ToString();
                        attachment2.Filename = attachment.FileName;
                        attachment2.Content = Convert.ToBase64String(attachment.Content);
                        attachment2.Type = attachment.ContentType;
                        if(attachment.IsInline)
                        {
                            attachment2.Disposition = "inline";
                            attachment2.ContentId = attachment.FileName;
                        }
                        else
                        {
                            attachment2.Disposition = "attachment";
                        }
                        msg.AddAttachment(attachment2);
                    }
                    //msg.Attachments.AddRange(attachmentList);
                }
                foreach (var recipient in email.Recipients)
                {
                    msg.AddTo(new EmailAddress(recipient));
                }
            
                var response = await client.SendEmailAsync(msg);
                if(response.IsSuccessStatusCode)
                {
                    return new EmailSentResult
                    {
                        Status = EmailStatus.OK
                    };
                }
                else
                {
                    return new EmailSentResult
                    {
                        Status = EmailStatus.FAILED
                    };
                }
            }
            catch (Exception ex)
            {
                _logger?.Error($"faild To send Email from : {ex.Message}/{ex.StackTrace}");
                return new EmailSentResult
                {
                    Status = EmailStatus.FAILED
                };
            }
        }
    }
}
