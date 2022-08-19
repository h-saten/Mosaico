using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Mosaico.Base.Abstractions;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Integration.Email.EmailLabs.Configurations;
using Mosaico.Integration.Email.EmailLabs.Exceptions;
using Mosaico.Integration.Email.EmailLabs.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Serilog;

namespace Mosaico.Integration.Email.EmailLabs
{
    public class EmailLabsClient : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly EmailLabsConfig _emailLabsConfig;
        private readonly IValidator<Abstraction.Email> _emailValidator;
        private readonly IEnumerable<IRestClientInterceptor> _clientInterceptors;

        public EmailLabsClient(EmailLabsConfig emailLabsConfig, IEnumerable<IRestClientInterceptor> clientInterceptors = null, IValidator<Abstraction.Email> emailValidator = null, ILogger logger = null)
        {
            _emailLabsConfig = emailLabsConfig;
            _clientInterceptors = clientInterceptors;
            _emailValidator = emailValidator;
            _logger = logger;
        }

        public async Task<EmailSentResult> SendAsync(Abstraction.Email email, CancellationToken t = new())
        {
            _logger?.Verbose($"Attempting to send email {email.Subject}");
            if (email == null)
            {
                throw new InvalidEmailPayloadException();
            }
            
            var client = await GetRestClientAsync();
            var config = GetBaseSendEmailConfig(email);
            ValidateEmail(email);
            _logger?.Verbose($"Email was successfully validated");
            var restRequest = BuildRequest(Method.POST, config);
            var response = await client.ExecutePostAsync<object>(restRequest, t);
            var emailLabsResponse = JsonConvert.DeserializeObject<EmailLabsResponse>(response.Content);
            _logger?.Verbose($"Received response from email labs: {emailLabsResponse?.Message}");
            if (!response.IsSuccessful || (emailLabsResponse != null && emailLabsResponse.Code != 200))
            {
                throw new EmailLabsDeliveryException($"Failed to send an email via email labs: {emailLabsResponse?.Message}");
            }
            _logger?.Verbose("Email was successfully sent");
            return new EmailSentResult
            {
                Status = EmailStatus.OK
            };
        }

        private void ValidateEmail(Abstraction.Email email)
        {
            if (_emailValidator != null)
            {
                var validationResult = _emailValidator.Validate(email);
                if (!validationResult.IsValid)
                {
                    var error = validationResult.Errors.FirstOrDefault();
                    throw new EmailValidationException($"Validation error: {error?.ErrorMessage} when trying to send {email.Subject}");
                }
            }
        }

        private async Task<IRestClient> GetRestClientAsync()
        {
            IRestClient client = new RestClient(_emailLabsConfig.Url)
            {
                Timeout = TimeSpan.FromSeconds(40).Milliseconds,
                Authenticator = new HttpBasicAuthenticator(_emailLabsConfig.AppKey, _emailLabsConfig.SecretKey)
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

        protected virtual RestRequest BuildRequest(Method method, Abstraction.Email email)
        {
            var requestApi = new RestRequest(Constants.Resources.NewEmail, DataFormat.Json);
            requestApi.OnBeforeDeserialization = resp =>
            {
                resp.ContentType = "application/json";
            };
            requestApi.Method = method;
            requestApi.AddHeader(Constants.Parameters.ApplicationKey, GetEncodedSecret());
            requestApi.AddParameter(Constants.Parameters.SMTPAccount, _emailLabsConfig.SmtpAccount);
            requestApi.AddParameter(Constants.Parameters.FromEmail, email.FromEmail);
            requestApi.AddParameter(Constants.Parameters.DisplayName, email.FromName);
            requestApi.AddParameter(Constants.Parameters.Subject, email.Subject);
            requestApi.AddParameter(Constants.Parameters.Html, email.Html);
            //TODO: check if number of recipients exceeds 250. if yes, then we should do a batch job
            foreach (var recipient in email.Recipients)
            {
                requestApi.AddParameter($"to[{recipient}]]", "");
            }

            if (email.Attachments != null)
            {
                foreach (var attachment in email.Attachments)
                {
                    var attachmentIndex = email.Attachments.IndexOf(attachment);
                    requestApi.AddParameter($"files[{attachmentIndex}][name]", $"{attachment.FileName}");
                    requestApi.AddParameter($"files[{attachmentIndex}][mime]", $"{attachment.ContentType}");
                    var attachmentAsBase64 = Convert.ToBase64String(attachment.Content);
                    requestApi.AddParameter($"files[{attachmentIndex}][content]", attachmentAsBase64);
                    if (attachment.IsInline)
                    {
                        requestApi.AddParameter($"files[{attachmentIndex}][inline]", "1");
                    }
                }
            }

            return requestApi;
        }

        protected virtual Abstraction.Email GetBaseSendEmailConfig(Abstraction.Email email)
        {
            if (email != null)
            {

                if (string.IsNullOrWhiteSpace(email.FromEmail))
                {
                    email.FromEmail = _emailLabsConfig.FromEmail;
                }

                if (string.IsNullOrWhiteSpace(email.FromName))
                {
                    email.FromName = _emailLabsConfig.DisplayName;
                }
                else
                {
                    email.FromName = _emailLabsConfig.FromEmail;
                }
            }

            return email;
        }

        protected virtual string GetEncodedSecret()
        {
            var appKeyAsBytes = Encoding.ASCII.GetBytes(_emailLabsConfig.SecretKey);
            return Convert.ToBase64String(appKeyAsBytes);
        }
        
    }
}