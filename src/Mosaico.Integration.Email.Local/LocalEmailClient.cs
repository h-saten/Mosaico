using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Storage.Base;
using Newtonsoft.Json;

namespace Mosaico.Integration.Email.Local
{
    public class LocalEmailClient : IEmailSender
    {
        private readonly IStorageClient _storageClient;

        public LocalEmailClient(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<EmailSentResult> SendAsync(Abstraction.Email email, CancellationToken t = new CancellationToken())
        {
            Console.WriteLine("********************************");
            Console.WriteLine(email.Html);
            Console.WriteLine("********************************");
            
            var now = DateTime.Now.ToString("s").Replace(":", "_");
            var folderName = $"{now}_{email.Subject}_{Guid.NewGuid()}";
            await _storageClient.CreateAsync($"{folderName}/email.html", Encoding.UTF8.GetBytes(email.Html), Constants.EmailContainer, false);
            var recipients = JsonConvert.SerializeObject(email, Formatting.Indented);
            await _storageClient.CreateAsync($"{folderName}/email.json", Encoding.UTF8.GetBytes(recipients), Constants.EmailContainer, false);
            if (email.Attachments != null)
            {
                foreach (var attachment in email.Attachments)
                {
                    await _storageClient.CreateAsync($"{folderName}/{attachment.FileName}", attachment.Content, Constants.EmailContainer, false);
                }
            }
            return new EmailSentResult
            {
                Status = EmailStatus.OK
            };
        }
    }
}