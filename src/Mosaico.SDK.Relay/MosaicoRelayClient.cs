using System;
using System.Threading.Tasks;
using Mosaico.SDK.Relay.Abstractions;
using Mosaico.SDK.Relay.Configurations;
using Mosaico.SDK.Relay.Exceptions;
using Mosaico.SDK.Relay.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Serilog;

namespace Mosaico.SDK.Relay
{
    public class MosaicoRelayClient : IMosaicoRelayClient
    {
        private readonly RelayConfig _relayConfig;
        private readonly ILogger _logger;

        public MosaicoRelayClient(RelayConfig relayConfig, ILogger logger)
        {
            _relayConfig = relayConfig;
            _logger = logger;
        }

        public async Task<string> StakeAsync(StakeParams parameters)
        {
            var request = new RestRequest("/api/milkycoin/staking/dividends/stake", Method.POST);
            request.AddJsonBody(parameters, "application/json");

            var response = await ExecuteRequestAsync<StakeResponse>(request);
            return response.Data;
        }

        public async Task<string> ClaimAsync(ClaimParams parameters)
        {
            var request = new RestRequest("/api/milkycoin/staking/dividends/claim", Method.POST);
            request.AddJsonBody(parameters, "application/json");

            var response = await ExecuteRequestAsync<ClaimResponse>(request);
            return response.Data;
        }

        public async Task<string> Withdraw(WithdrawParams parameters)
        {
            var request = new RestRequest("/api/milkycoin/staking/dividends/withdraw", Method.POST);
            request.AddJsonBody(parameters, "application/json");
            var response = await ExecuteRequestAsync<WithdrawResponse>(request);
            return response.Data;
        }

        private async Task<T> ExecuteRequestAsync<T>(RestRequest request)
        {
            var client = GetClient();
            try
            {
                var response = await client.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                    throw new Exception(JsonConvert.DeserializeObject<RelayErrorResponse>(response.Content)?.Message);
                }

                var contentResponse = JsonConvert.DeserializeObject<T>(response.Content);
                if (contentResponse == null)
                {
                    throw new Exception("Relay does not contain proper body to handle response");
                }

                return contentResponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, $"Error during relay operation");
                throw new RelayClientException(ex.Message);;
            }
        }

        private RestClient GetClient()
        {
            var client = new RestClient(_relayConfig.Uri);
            client.UseNewtonsoftJson();
            client.AddDefaultHeader("Authorization", $"Api-Key {_relayConfig.ApiKey}");
            return client;
        }

        public async Task<decimal> AllowanceAsync(AllowanceParams parameters)
        {
            var request = new RestRequest("/api/milkycoin/tokens/allowance", Method.GET);
            request.Parameters.Add(new Parameter("userId", parameters.UserId, ParameterType.QueryString));
            request.Parameters.Add(new Parameter("owner", parameters.Owner, ParameterType.QueryString));
            request.Parameters.Add(new Parameter("spender", parameters.Spender, ParameterType.QueryString));
            var response = await ExecuteRequestAsync<AllowanceResponse>(request);
            return response.Data;
        }
    }
}