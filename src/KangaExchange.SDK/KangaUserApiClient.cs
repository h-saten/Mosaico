using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using KangaExchange.SDK.Exceptions;
using KangaExchange.SDK.Models;
using KangaExchange.SDK.Models.Profile;
using RestSharp;
using Serilog;

namespace KangaExchange.SDK
{
    public class KangaUserApiClient : APIClientBase, IKangaUserApiClient
    {
        private readonly ILogger _logger;
        
        public KangaUserApiClient(ISignatureService signatureGenerateService, KangaConfiguration kangaConfiguration, ILogger logger = null) :
            base(signatureGenerateService, kangaConfiguration)
        {
            _logger = logger;
        }

        public async Task<(bool success, string errorCode)> PasswordRecoveryAsync(string email)
        {
            var apiRequestUrl = "v2/auth/password/recovery";

            var timestamp = TimestampRawNonce();

            var body = new
            {
                appId = GetAppId(),
                nonce = timestamp,
                email
            };

            var requestApi = CreateRequest(apiRequestUrl, body);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<PasswordRecoveryResponse>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error($"[{nameof(KangaUserApiClient)}] Error while create account for '{email}'.");
                throw new KangaException("kanga_api_connect_error");
            }

            if (responseApi.Data.Result != "ok")
            {
                var errorCode = responseApi.Data.Code;

                if (errorCode == "9000") _logger?.Error($"[{nameof(KangaUserApiClient)}] invalid signature");
                if (errorCode == "9001") _logger?.Error($"[{nameof(KangaUserApiClient)}] access denied");

                throw new KangaException(errorCode, "Request error while reset password in Kanga");
            }

            _logger?.Information("Successfully reset password request in Kanga.");

            return (
                responseApi.Data.Result == "ok",
                responseApi.Data.Code
            );
        }

        Task<(bool success, UserProfileResponseDto, string errorCode)> IKangaUserApiClient.UserProfile(
            string kangaUserId)
        {
            return UserProfile(kangaUserId);
        }

        private string MapLanguageEnumToString(Language language)
        {
            if (language == Language.EN) return "EN";
            if (language == Language.PL) return "PL";

            throw new ArgumentException("Invalid language enum value");
        }

        public async Task<(string kangaUserId, string resetPasswordUrl)> CreateAccountAsync(string email, Language language)
        {
            var apiRequestUrl = "v2/account/create";

            var timestamp = TimestampRawNonce();

            var body = new
            {
                appId = GetAppId(),
                nonce = timestamp,
                email,
                language = MapLanguageEnumToString(language),
                recommender = "3f4fbb8c-3f0c-471e-9d01-d8b3458187e9" //TODO: what is this
            };

            var requestApi = CreateRequest(apiRequestUrl, body);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<CreateUserResponseDto>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error($"[{nameof(KangaUserApiClient)}] Error while create account for '{email}'.");
                throw new KangaException("kanga_api_connect_error");
            }

            if (responseApi.Data.Result != "ok")
            {
                var errorCode = responseApi.Data.Code;

                if (errorCode == "9000") _logger?.Error($"[{nameof(KangaUserApiClient)}] invalid signature");
                if (errorCode == "9001") _logger?.Error($"[{nameof(KangaUserApiClient)}] access denied");
                if (errorCode == "9002") _logger?.Error($"[{nameof(KangaUserApiClient)}] create user failed");

                throw new KangaException(errorCode, "Request error while get report from Kanga");
            }

            _logger?.Information("Successfully create account in Kanga request.");

            return (
                responseApi.Data.AppUserId,
                responseApi.Data.PasswordUrl
            );
        }

        public async Task<(bool success, UserProfileResponseDto, string errorCode)> UserProfile(string kangaUserId)
        {
            var apiRequestUrl = "v2/oauth2/profile";

            var body = new
            {
                appId = GetAppId(),
                appUserId = kangaUserId
            };

            var requestApi = CreateRequest(apiRequestUrl, body);
            var restClient = new RestClient(GetBaseUrl());
            var responseApi = await restClient.ExecutePostAsync<RawUserProfileResponseDto>(requestApi);

            if (!responseApi.IsSuccessful)
            {
                _logger?.Error(
                    $"[{nameof(KangaUserApiClient)}] Error while get profile for kanga user: '{kangaUserId}'.");
                throw new KangaException("kanga_api_connect_error");
            }

            if (responseApi.Data.Result != "ok")
            {
                var errorCode = responseApi.Data.Code;

                if (errorCode == "9000") _logger?.Error("Invalid signature");
                if (errorCode == "9001") _logger?.Error("No permissions for app");
                if (errorCode == "9002") _logger?.Error("Unknown app user id");

                throw new KangaException(errorCode, "Request error while get Kanga user profile");
            }

            _logger?.Information("Successfully request to Kanga profile endpoint.");

            var userData = await ProcessProfileData(responseApi.Data);

            return (
                responseApi.Data.Result == "ok",
                userData,
                responseApi.Data.Code
            );
        }

        private async Task<UserProfileResponseDto> ProcessProfileData(RawUserProfileResponseDto data)
        {
            var result = new UserProfileResponseDto
            {
                Personal = data.Personal,
                Company = data.Company,
                Email = data.Email,
                Kyc = data.Kyc
            };

            var addressesList = new List<UserAddressDto>();

            var walletsRaw = data.Wallet;
            var addressesRaw = data.Addresses;

            if (walletsRaw != null) result.Wallet = await UserTokenWalletBalances(walletsRaw);

            if (addressesRaw != null)
            {
                RawUserAddressDto rawEntry = null;

                rawEntry = addressesRaw.Find(m => m.ETHER != null);
                var isLegacy = rawEntry?.ETHER.Legacy != null;
                var address = rawEntry?.ETHER.Legacy != null ? rawEntry?.ETHER.Legacy : "NIELEGACY";
                if (rawEntry != null) addressesList.Add(new UserAddressDto("ETH", address, isLegacy));

                rawEntry = addressesRaw.Find(m => m.BITCOIN != null);
                isLegacy = rawEntry?.BITCOIN.Legacy != null;
                address = rawEntry?.BITCOIN.Legacy != null ? rawEntry?.BITCOIN.Legacy : "NIELEGACY";
                if (rawEntry != null) addressesList.Add(new UserAddressDto("BTC", address, isLegacy));
            }

            result.Addresses = addressesList;

            return result;
        }

        private Task<List<UserWalletEntryDto>> UserTokenWalletBalances(Dictionary<string, string> walletsRaw)
        {
            var walletsBalances = new List<UserWalletEntryDto>();
            foreach (var wallet in walletsRaw)
            {
                var tokenWallet = new UserWalletEntryDto(wallet.Key, wallet.Value, true);
                walletsBalances.Add(tokenWallet);
            }

            return Task.FromResult(walletsBalances);
        }
    }
}