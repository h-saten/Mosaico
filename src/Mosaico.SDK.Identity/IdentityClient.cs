using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using IdentityModel.Client;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Models;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.SDK.Base.Exceptions;
using Mosaico.SDK.Base.Models;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Configurations;
using Mosaico.SDK.Identity.Models;
using Newtonsoft.Json;
using Serilog;

namespace Mosaico.SDK.Identity
{
    public class IdentityClient : IUserManagementClient
    {
        private readonly ICurrentUserContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IdentityServerConfiguration _configuration;
        private readonly IIdentityContext _identityDbContext; 
        private readonly IKangaUserApiClient _kangaUserApiClient;
        private readonly ILogger _logger;
        
        public IdentityClient(ICurrentUserContext context, IHttpClientFactory httpClientFactory, IdentityServerConfiguration configuration, IIdentityContext identityDbContext, IKangaUserApiClient kangaUserApiClient, ILogger logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _identityDbContext = identityDbContext;
            _kangaUserApiClient = kangaUserApiClient;
            _logger = logger;
        }

        public async Task<MosaicoUser> GetCurrentUserAsync(CancellationToken cToken = new CancellationToken())
        {
            if (!_context.IsAuthenticated) 
                return null;
            
            var currentUserId = _context.UserId;
            using (var client = _httpClientFactory.CreateClient())
            {
                var accessToken = await _context.GetAccessTokenAsync();
                var url = Url.Combine(_configuration.Url, "id", $"/api/users/{currentUserId}");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                request.SetBearerToken(accessToken);
                var response = await client.SendAsync(request, cToken);
                var content = await response.Content.ReadAsStringAsync(cToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new MosaicoException(content);
                }

                var successResponse = JsonConvert.DeserializeObject<SuccessResponse<MosaicoUser>>(content);
                if (successResponse == null || !successResponse.Ok)
                {
                    throw new MosaicoException("Failed user request");
                }
                return successResponse.Data;
            }
        }

        public async Task<MosaicoUser> GetUserAsync(string identifier, CancellationToken token = new CancellationToken())
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var accessToken = await GetInternalAccessTokenAsync();
                var url = Url.Combine(_configuration.Url, "id", $"/api/users/{identifier}");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                request.SetBearerToken(accessToken);
                var response = await client.SendAsync(request, token);
                var content = await response.Content.ReadAsStringAsync(token);
                if (!response.IsSuccessStatusCode)
                {
                    throw new MosaicoException(content);
                }

                var successResponse = JsonConvert.DeserializeObject<SuccessResponse<MosaicoUser>>(content);
                if (successResponse == null || !successResponse.Ok)
                {
                    throw new MosaicoException("Failed user request");
                }
                return successResponse.Data;
            }
        }
        
        public async Task<List<MosaicoUser>> GetUsersByNameAsync(string userName, CancellationToken token = new CancellationToken())
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var accessToken = await GetInternalAccessTokenAsync();
                var url = Flurl.Url.Combine(_configuration.Url, "id", $"/api/users/{userName}/user");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                
                request.SetBearerToken(accessToken);
                var response = await client.SendAsync(request, token);
                var content = await response.Content.ReadAsStringAsync(token);
                if (!response.IsSuccessStatusCode)
                {
                    throw new MosaicoException(content);
                }

                var successResponse = JsonConvert.DeserializeObject<SuccessResponse<List<MosaicoUser>>>(content);
                if (successResponse == null || !successResponse.Ok)
                {
                    throw new MosaicoException("Failed user request");
                }
                return successResponse.Data;
            }
        }

        public async Task<List<MosaicoPermission>> GetUserPermissionsAsync(string identifier, CancellationToken cToken = new CancellationToken())
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var accessToken = await GetInternalAccessTokenAsync();
                var url = Url.Combine(_configuration.Url, "id", $"/api/users/{identifier}/permissions");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                request.SetBearerToken(accessToken);
                var response = await client.SendAsync(request, cToken);
                var content = await response.Content.ReadAsStringAsync(cToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new MosaicoException(content);
                }
                var successResponse = JsonConvert.DeserializeObject<SuccessResponse<UserPermissionResponse>>(content);
                if (successResponse == null || !successResponse.Ok)
                {
                    throw new MosaicoException("Failed user request");
                }
                return successResponse.Data?.Permissions;
            }
        }

        public async Task<List<MosaicoPermission>> GetUserPermissionsAsync(string identifier, Guid entityId, CancellationToken cToken = new CancellationToken())
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var accessToken = await GetInternalAccessTokenAsync();
                var url = Url.Combine(_configuration.Url, "id", $"/api/users/{identifier}/permissions?entityId={entityId}");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                };
                request.SetBearerToken(accessToken);
                var response = await client.SendAsync(request, cToken);
                var content = await response.Content.ReadAsStringAsync(cToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new MosaicoException(content);
                }
                var successResponse = JsonConvert.DeserializeObject<SuccessResponse<UserPermissionResponse>>(content);
                if (successResponse == null || !successResponse.Ok)
                {
                    throw new MosaicoException("Failed user request");
                }
                return successResponse.Data?.Permissions;
            }
        }

        public virtual async Task<List<MosaicoUser>> GetUsersWithPermission(string key, CancellationToken cToken = new CancellationToken())
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var accessToken = await GetInternalAccessTokenAsync();
                var url = Url.Combine(_configuration.Url, "id", $"/api/users/permissions/{key}");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                };
                request.SetBearerToken(accessToken);
                var response = await client.SendAsync(request, cToken);
                var content = await response.Content.ReadAsStringAsync(cToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new MosaicoException(content);
                }
                var successResponse = JsonConvert.DeserializeObject<SuccessResponse<List<MosaicoUser>>>(content);
                if (successResponse == null || !successResponse.Ok)
                {
                    throw new MosaicoException("Failed user request");
                }
                return successResponse.Data;
            }
        }
        
        public async Task<bool> AccountExist(string email)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var url = Url.Combine(_configuration.Url, "id", $"/api/auth/ApiAccount/AccountExist?email={email}");
                var responseMessage = await client.GetAsync(url);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new MosaicoException("api_error");
                }

                var response = await responseMessage.Content.ReadAsStringAsync();
                var responseItem = JsonConvert.DeserializeObject<SuccessResponse<bool>>(response);
                return responseItem.Data;
            }
        }
        
        public async Task<bool> RegisterAccountAsync(string email, string password, string language, bool joinNewsletter = false)
        {
            var url = Url.Combine(_configuration.Url, "id", $"/api/auth/apiAccount/Register");
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };

            var body = new {
                email = email,
                password = password,
                confirmPassword = password,
                terms = true,
                notForbiddenCitizenship = true,
                newsletterPersonalDataProcessing = joinNewsletter,
                language = language
            };
            request.Headers.Add("content-type", "application/json");
            request.Content = new StringContent(JsonConvert.SerializeObject(body));
            using (var client = _httpClientFactory.CreateClient())
            {
                var responseApi = await client.SendAsync(request);

                if (!responseApi.IsSuccessStatusCode)
                {
                    _logger?.Error($"Error while try register user {email}.");
                    throw new MosaicoException("register_error");
                }

                _logger?.Information($"Successfully send register request.");

                return true;
            }
        }

        public async Task<bool> RegisterConfirmedAccountAsync(string email, string password, string language, bool joinNewsletter = false)
        {
            var url = Url.Combine(_configuration.Url, "id", $"/api/auth/ApiAccount/RegisterConfirmed");
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };
            var body = new {
                email = email,
                password = password,
                confirmPassword = password,
                terms = true,
                notForbiddenCitizenship = true,
                newsletterPersonalDataProcessing = joinNewsletter,
                language = language
            };
            request.Headers.Add("content-type", "application/json");
            request.Content = new StringContent(JsonConvert.SerializeObject(body));
            using (var client = _httpClientFactory.CreateClient())
            {
                var token = await GetInternalAccessTokenAsync();
                client.SetBearerToken(token);
                var responseApi = await client.SendAsync(request);

                if (!responseApi.IsSuccessStatusCode)
                {
                    _logger?.Error($"Error while try register user {email}.");
                    throw new MosaicoException("register_error");
                }

                _logger?.Information($"Successfully send register request.");

                return true;
            }
        }

        private async Task<string> GetInternalAccessTokenAsync()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
                {
                    Address = $"{_configuration.Authority}/connect/token",
                    ClientId = _configuration.Api?.ClientId,
                    ClientSecret = $"{_configuration.Api?.ClientSecret}",
                    Scope = "IdentityServerApi"
                });
                return tokenResponse.AccessToken;
            }
        }

        public async Task<bool> CreateKangaUserIfNotExist(string identifier, CancellationToken token = new CancellationToken())
        {
            var appUserCounter = await _identityDbContext
                .Users
                .CountAsync(cancellationToken: token);
                
            var appUser = await _identityDbContext
                .Users
                .Where(x => x.Id == identifier)
                .SingleOrDefaultAsync(cancellationToken: token);

            if (appUser is null)
            {
                throw new MosaicoException($"Account: {identifier} not exist");
            }

            var email = appUser.Email;
            
            var accountAlreadyExists = await _identityDbContext
                .KangaUsers
                .Where(m => m.Email == email)
                .AnyAsync(cancellationToken: token);
            
            if (accountAlreadyExists)
            {
                return false;
            }
            
            try
            {
                (string kangaUserId, string resetPasswordUrl) = await _kangaUserApiClient
                    .CreateAccountAsync(email, Language.EN);

                var account = new KangaUser
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    ApplicationUserId = appUser.Id,
                    ApplicationUser = appUser,
                    CreatedAt = DateTime.UtcNow,
                    KycVerified = false,
                    KangaAccountId = kangaUserId
                };

                await _identityDbContext.KangaUsers.AddAsync(account);
                await _identityDbContext.SaveChangesAsync();
                
                // SEND EMAIL WITH PASSWORD RESET

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task<MosaicoUser> GetUserByEmailAsync(string email, CancellationToken token = new())
        {
            using (var client = _httpClientFactory.CreateClient(_configuration.Api.ClientName))
            {
                var url = Url.Combine(_configuration.Url, "id", $"/api/users/find?findBy=email&value={email}");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                var response = await client.SendAsync(request, token);
                var content = await response.Content.ReadAsStringAsync(token);
                if (!response.IsSuccessStatusCode)
                {
                    throw new MosaicoException(content);
                }

                var successResponse = JsonConvert.DeserializeObject<SuccessResponse<MosaicoUser>>(content);
                if (successResponse == null || !successResponse.Ok)
                {
                    throw new MosaicoException("Failed user request");
                }
                return successResponse.Data;
            }
        }
        
        public async Task<KangaUserAccount> GetUserKangaAccountAsync(CancellationToken cToken = new())
        {
            if (!_context.IsAuthenticated) 
                return null;
            
            var currentUserId = _context.UserId;
            using (var client = _httpClientFactory.CreateClient())
            {
                var accessToken = await _context.GetAccessTokenAsync();
                var url = Url.Combine(_configuration.Url, "id", $"/api/auth/Kanga/KangaUser/{currentUserId}");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                request.SetBearerToken(accessToken);
                var response = await client.SendAsync(request, cToken);
                var content = await response.Content.ReadAsStringAsync(cToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new MosaicoException(content);
                }

                var successResponse = JsonConvert.DeserializeObject<SuccessResponse<KangaUserAccount>>(content);
                if (successResponse == null || !successResponse.Ok)
                {
                    throw new MosaicoException("Failed user request");
                }
                return successResponse.Data;
            }
        }

        public async Task<List<MosaicoUser>> GetUsersAsync(List<string> identifiers, CancellationToken token = new())
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var accessToken = await GetInternalAccessTokenAsync();
                var url = Url.Combine(_configuration.Url, "id", "/api/users/select");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post
                };
                request.Content = new StringContent(JsonConvert.SerializeObject(identifiers), Encoding.UTF8, "application/json");
                request.SetBearerToken(accessToken);
                var response = await client.SendAsync(request, token);
                var content = await response.Content.ReadAsStringAsync(token);
                if (!response.IsSuccessStatusCode)
                {
                    throw new MosaicoException(content);
                }

                var successResponse = JsonConvert.DeserializeObject<SuccessResponse<SelectUserResponse>>(content);
                if (successResponse == null || !successResponse.Ok)
                {
                    throw new MosaicoException("Failed user request");
                }
                return successResponse.Data.Users;
            }
        }
        
        public async Task<string> CreateExternalAccountAsync(string email)
        {
            var url = Url.Combine(_configuration.Url, "id", "/api/auth/ApiAccount/RegisterExternalUser");
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };

            var body = new {
                email
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            using (var client = _httpClientFactory.CreateClient(_configuration.Api.ClientName))
            {
                var responseApi = await client.SendAsync(request);
                var responseContent = await responseApi.Content.ReadAsStringAsync();
                var responseResult = JsonConvert.DeserializeObject<SuccessResponse<string>>(responseContent);

                if (!responseApi.IsSuccessStatusCode)
                {
                    _logger?.Error($"Error while try register user {email}.");
                    throw new MosaicoException("register_error");
                }

                _logger?.Information($"Successfully send register request.");

                return responseResult.Data;
            }
        }
        
    }
}