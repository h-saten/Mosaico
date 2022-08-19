using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Base.Abstractions;
using Mosaico.Integration.UserCom.Abstractions;
using Mosaico.Integration.UserCom.Configurations;
using Mosaico.Integration.UserCom.Exceptions;
using Mosaico.Integration.UserCom.Extensions;
using Mosaico.Integration.UserCom.Models.Request;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Mosaico.Integration.UserCom
{
    public class ConversationsClient : IUserComConversationsClient
    {
        private readonly ILogger _logger;
        private readonly UserComConfig _userComConfig;
        private readonly IEnumerable<IRestClientInterceptor> _clientInterceptors;

        public ConversationsClient(UserComConfig userComConfig, IEnumerable<IRestClientInterceptor> clientInterceptors = null, ILogger logger = null)
        {
            _userComConfig = userComConfig;
            _clientInterceptors = clientInterceptors;
            _logger = logger;
        }

        public async Task CreateConversationAsync(CreateConversationParams parameters, CancellationToken t = new())
        {
            var client = await GetRestClientAsync();
            
            // Get user id
            // if not exists, create user and in return receive user id
            var body = new CreateConversation
            {
                Content = parameters.Message,
                Source = "3", // API
                AssignedTo = 25,
                SourceContext = $"Contact form. User name: {parameters.Name}",
                UserId = parameters.UserId
            };
            var restRequest = BuildRequest(body);
            var response = await client.ExecuteAsync<object>(restRequest, t);
            _logger?.Verbose($"Received response from user.com");
            if (!response.IsSuccessful)
            {
                throw new UserComException($"Failed to execute 'create conversation' request.");
            }
            _logger?.Verbose("Email was successfully sent");
        }
        
        private async Task<IRestClient> GetRestClientAsync()
        {
            IRestClient client = new RestClient(_userComConfig.Url)
            {
                Timeout = TimeSpan.FromSeconds(40).Milliseconds
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

        protected virtual RestRequest BuildRequest(CreateConversation body)
        {
            var requestApi = new RestRequest($"users/{body.UserId}/conversations/", DataFormat.Json);
            requestApi.OnBeforeDeserialization = resp =>
            {
                resp.ContentType = "application/json";
            };
            requestApi.Method = Method.POST;
            requestApi.Authorize(_userComConfig);
            var serializedBody = JsonConvert.SerializeObject(body);
            requestApi.AddParameter("application/json", serializedBody, ParameterType.RequestBody);

            return requestApi;
        }
        
    }
}