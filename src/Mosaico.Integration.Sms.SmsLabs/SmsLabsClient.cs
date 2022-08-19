using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Base.Abstractions;
using Mosaico.Integration.Sms.Abstraction;
using Mosaico.Integration.Sms.SmsLabs.Configurations;
using Mosaico.Integration.Sms.SmsLabs.Exceptions;
using Mosaico.Integration.Sms.SmsLabs.Models;
using Mosaico.Integration.Sms.SmsLabs.Models.SendSms;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Serilog;

namespace Mosaico.Integration.Sms.SmsLabs
{
    public class SmsLabsClient : ISmsSender
    {
        private readonly ILogger _logger;
        private readonly SmsLabsConfig _smsLabsConfig;
        private readonly IEnumerable<IRestClientInterceptor> _clientInterceptors;

        public SmsLabsClient(SmsLabsConfig smsLabsConfig, IEnumerable<IRestClientInterceptor> clientInterceptors = null, ILogger logger = null)
        {
            _smsLabsConfig = smsLabsConfig;
            _clientInterceptors = clientInterceptors;
            _logger = logger;
        }

        public async Task<SmsSentResult> SendAsync(SmsMessage message, CancellationToken t = new())
        {
            _logger?.Verbose($"Attempting to send sms {message.Subject}");
            if (message == null)
            {
                throw new InvalidSmsPayloadException();
            }
            
            var client = await GetRestClientAsync();
            _logger?.Verbose($"Email was successfully validated");
            var restRequest = BuildRequest(Method.POST, message);
            var response = await client.ExecutePostAsync<object>(restRequest, t);
            var emailLabsResponse = JsonConvert
                .DeserializeObject<SmsLabsResponse<SendSmsDataDto, SendSmsErrorDto>>(response.Content);
            _logger?.Verbose($"Received response from sms labs: {emailLabsResponse?.Meta.RequestId}");
            if (!response.IsSuccessful || (emailLabsResponse != null && emailLabsResponse.Data == null))
            {
                throw new SmsLabsDeliveryException(emailLabsResponse?.Errors.First().Title);
            }
            _logger?.Verbose("Sms was successfully sent");
            return new SmsSentResult
            {
                Status = SmsStatus.OK
            };
        }

        private async Task<IRestClient> GetRestClientAsync()
        {
            IRestClient client = new RestClient(_smsLabsConfig.Url)
            {
                Timeout = TimeSpan.FromSeconds(40).Milliseconds,
                Authenticator = new HttpBasicAuthenticator(_smsLabsConfig.AppKey, _smsLabsConfig.SecretKey)
            };
            if (_clientInterceptors != null)
            {
                foreach (var interceptor in _clientInterceptors)
                {
                    client = await interceptor.InterceptAsync(client);
                }
            }
            return client;
        }

        protected virtual RestRequest BuildRequest(Method method, SmsMessage message)
        {
            var requestApi = new RestRequest(Constants.Resources.SendSms, DataFormat.Json);
            requestApi.OnBeforeDeserialization = resp =>
            {
                resp.ContentType = "application/json";
            };
            requestApi.Method = method;
            
            requestApi.AddHeader("Authorization", $"Basic {GetEncodedSecret()}");

            requestApi.AddBody(new
            {
                phone_number = message.RecipientsPhoneNumber,
                sender_id = _smsLabsConfig.SenderId,
                message = message.Content
            });

            return requestApi;
        }

        protected virtual string GetEncodedSecret()
        {
            var appKeyAsBytes = Encoding.ASCII.GetBytes(_smsLabsConfig.SecretKey + ":" + _smsLabsConfig.AppKey);
            return Convert.ToBase64String(appKeyAsBytes);
        }
    }
}