using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Base.Abstractions;
using Mosaico.Integration.UserCom.Abstractions;
using Mosaico.Integration.UserCom.Configurations;
using Mosaico.Integration.UserCom.Exceptions;
using Mosaico.Integration.UserCom.Extensions;
using Mosaico.Integration.UserCom.Models;
using Mosaico.Integration.UserCom.Models.Request;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Mosaico.Integration.UserCom
{
    public class UsersClient : IUserComUsersApiClient
    {
        private readonly ILogger _logger;
        private readonly UserComConfig _userComConfig;
        private readonly IEnumerable<IRestClientInterceptor> _clientInterceptors;

        public UsersClient(UserComConfig userComConfig, IEnumerable<IRestClientInterceptor> clientInterceptors = null, ILogger logger = null)
        {
            _userComConfig = userComConfig;
            _clientInterceptors = clientInterceptors;
            _logger = logger;
        }

        public async Task<FindUserResponse> FindUserByEmailAsync(string email, CancellationToken t = new())
        {
            var client = await GetRestClientAsync();
            var restRequest = BuildFindUserRequest(email);
            var response = await client.ExecuteAsync<FindUserResponse>(restRequest, t);
            _logger?.Verbose($"Received response from user.com");
            if (!response.IsSuccessful)
            {
                _logger?.Information($"User not found.");
                return null;
            }
            _logger?.Verbose("User data was successfully fetched");
            return response.Data;
        }

        public async Task<CreateUserResponse> CreateUserAsync(string email, string name, CancellationToken t = new())
        {
            var client = await GetRestClientAsync();
            var restRequest = BuildCreateUserRequest(new CreateUser {Email = email, FirstName = name});
            var response = await client.ExecuteAsync<CreateUserResponse>(restRequest, t);
            _logger?.Verbose($"Received response from user.com");
            if (!response.IsSuccessful)
            {
                throw new UserComException($"Failed to execute 'create user' request.");
            }
            _logger?.Verbose("User was successfully created");
            return response.Data;
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

        protected virtual RestRequest BuildFindUserRequest(string emailAddress)
        {
            var requestApi = new RestRequest("users/search/");
            requestApi.Method = Method.GET;
            requestApi.Authorize(_userComConfig);
            requestApi.AddQueryParameter("email", emailAddress.ToLower());

            return requestApi;
        }

        protected virtual RestRequest BuildCreateUserRequest(CreateUser body)
        {
            var requestApi = new RestRequest($"users/", DataFormat.Json);
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