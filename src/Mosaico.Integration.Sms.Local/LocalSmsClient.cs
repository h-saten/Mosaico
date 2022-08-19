using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Integration.Sms.Abstraction;
using Mosaico.Storage.Base;
using Newtonsoft.Json;
using Serilog;

namespace Mosaico.Integration.Sms.Local
{
    public class LocalSmsClient : ISmsSender
    {
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;
        public LocalSmsClient(IStorageClient storageClient, ILogger logger = null)
        {
            _storageClient = storageClient;
            _logger = logger;
        }
        
        public async Task<SmsSentResult> SendAsync(SmsMessage message, CancellationToken t = new())
        {
            _logger?.Warning("********************************");
            _logger?.Warning($">> SMS CODE: {message.Content}");
            _logger?.Warning("********************************");
            var now = DateTime.Now.ToString("s").Replace(":", "_");
            var folderName = $"{now}_{message.Subject.Replace(" ", "_").ToLowerInvariant()}_{Guid.NewGuid()}";
            await _storageClient.CreateAsync($"{folderName}/{message.Content}.json", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)), Constants.SmsContainer, false);
            await Task.CompletedTask;
            return new SmsSentResult
            {
                Status = SmsStatus.OK
            };
        }
    }
}