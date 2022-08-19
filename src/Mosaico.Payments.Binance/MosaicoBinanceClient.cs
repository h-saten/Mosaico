using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Payments.Binance.Abstractions;
using Mosaico.Payments.Binance.Configurations;
using Mosaico.Payments.Binance.Exceptions;
using Mosaico.Payments.Binance.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Mosaico.Payments.Binance
{
    public class MosaicoBinanceClient : IBinanceClient
    {
        private readonly BinanceConfiguration _binanceConfiguration;

        public MosaicoBinanceClient(BinanceConfiguration binanceConfiguration)
        {
            _binanceConfiguration = binanceConfiguration;
        }
        
        public async Task<BinanceOrderResponse> GetOrderAsync(BinanceOrderRequest payload, CancellationToken token = new CancellationToken())
        {
            var url = $"/binancepay/openapi/v2/order/query";
            var client = new RestClient(_binanceConfiguration.Url);
            var request = FormPostRequest(payload, url);
            var response = await client.ExecuteAsync(request, token);
            if (!response.IsSuccessful)
            {
                throw new BinanceException((int) response.StatusCode, response.Content);
            }
            var data = JsonConvert.DeserializeObject<BinanceResponse<BinanceOrderResponse>>(response.Content);
            if (data?.Status != "SUCCESS")
            {
                throw new BinanceException((int) response.StatusCode, data?.ErrorMessage);
            }

            return data.Data;
        }

        private RestRequest FormPostRequest<T>(T payload, string url)
        {
            var nonce = GenerateNonce().Substring(0, 32);
            var body = JsonConvert.SerializeObject(payload, Formatting.None);
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            var payloadToSign = timestamp + '\n' + nonce + '\n' + body + '\n';
            var signature = HashPayload(payloadToSign);
            var request = new RestRequest(url, Method.POST);
            request.AddHeaders(new List<KeyValuePair<string, string>>
            {
                new("BinancePay-Timestamp", timestamp),
                new("BinancePay-Nonce", nonce),
                new("BinancePay-Signature", signature),
                new("BinancePay-Certificate-SN", _binanceConfiguration.ApiKey),
                new("content-type", "application/json")
            });
            request.AddParameter("", body, ParameterType.RequestBody);
            return request;
        }

        public async Task<BinanceOrderCreationResponse> CreateOrderAsync(BinanceOrderCreationRequest payload, CancellationToken token = new CancellationToken())
        {
            var url = $"/binancepay/openapi/v2/order";
            var request = FormPostRequest(payload, url);
            var client = new RestClient(_binanceConfiguration.Url);
            var response = await client.ExecuteAsync(request, token);
            if (!response.IsSuccessful)
            {
                throw new BinanceException((int) response.StatusCode, response.Content);
            }

            var data = JsonConvert.DeserializeObject<BinanceResponse<BinanceOrderCreationResponse>>(response.Content);
            if (data?.Status != "SUCCESS")
            {
                throw new BinanceException((int) response.StatusCode, data?.ErrorMessage);
            }

            return data.Data;
        }

        private string HashPayload(string payload)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_binanceConfiguration.ApiSecret);
            using var encryptor = new HMACSHA512(keyBytes);
            var resultBytes = encryptor.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return resultBytes.Aggregate(string.Empty, (current, t) => current + t.ToString("X2"));
        }
        
        private string GenerateNonce(long size = 32)
        {
            using RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            var tokenData = new byte[size];
            rng.GetBytes(tokenData);
            var base64String = Convert.ToBase64String(tokenData);
            return base64String.Replace("/", "a")
                .Replace("+", "b")
                .Replace("=", "c")
                .Replace("-", "d");
        }
    }
}