using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Integration.Sms.Abstraction
{
    public interface ISmsSender
    {
        Task<SmsSentResult> SendAsync(SmsMessage message, CancellationToken t = new());
    }
}