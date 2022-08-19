using System.Collections.Generic;

namespace Mosaico.Integration.Email.Abstraction
{
    public class Email
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public List<string> Recipients { get; set; } = new List<string>();
        public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
    }
}