using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Integration.Email.Abstraction
{
    public interface IEmailSender
    {
        Task<EmailSentResult> SendAsync(Email email, CancellationToken t = new());
    }
}