using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using Mosaico.Identity.Configurations;
using static Mosaico.API.Base.Constants.Permissions;

namespace Mosaico.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }
        
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new("tokenizerapi.full", "Tokenizer api full access."),
                new(IdentityServerConstants.LocalApi.ScopeName, "Identity extra api"),
                new(AppServicesInternalActions, "Services internal operations"),
            };
        }

        public static IEnumerable<ApiResource> GetApis(IdentityServiceConfiguration configuration)
        {
            return new ApiResource[]
            {
                new ApiResource("tokenizerapi", "Tokenizer Resource")
                {
                    Scopes =
                    {
                        "tokenizerapi.full"
                    },
                    ApiSecrets =
                    {
                        new Secret(configuration.ApiClientSecret.Sha256())
                    },
                    UserClaims = { "role", JwtClaimTypes.EmailVerified, JwtClaimTypes.Name, JwtClaimTypes.Email, JwtClaimTypes.Id }
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
                {
                    ApiSecrets =
                    {
                        new Secret(configuration.IdentityClientSecret.Sha256())
                    },
                    Scopes = { IdentityServerConstants.LocalApi.ScopeName }
                }
            };
        }

        public static IEnumerable<Client> GetClients(IdentityServiceConfiguration configuration)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "tokenizerapiapp",
                    RequireConsent = false,
                    ClientSecrets =
                    {
                        new Secret(configuration.IdentityClientSecret.Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes =
                    {
                        "openid", "profile", "email", IdentityServerConstants.LocalApi.ScopeName, AppServicesInternalActions
                    },
                    Claims = new List<ClientClaim>
                    {
                        new(JwtClaimTypes.Actor, "adminapi")
                    }
                    
                },
                new Client
                {
                    ClientId = "mosaico_mobile",
                    RequireConsent = false,
                    AccessTokenType = AccessTokenType.Reference,
                    ClientSecrets =
                    {
                        new Secret(configuration.IdentityClientSecret.Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        "openid", "profile", "email", IdentityServerConstants.LocalApi.ScopeName, "offline_access"
                    },
                    AllowOfflineAccess = true,
                    IdentityProviderRestrictions = new List<string>
                    {
                        "Facebook",
                        "Google"
                    },
                    AccessTokenLifetime = 30 * 24 * 60 * 60, // 30days
                    IdentityTokenLifetime = 30 * 24 * 60 * 60,
                },
                new Client
                {
                    ClientId = "spa-tokenizer",
                    ClientName = "Tokenizer WEB.UI",
                    AccessTokenType = AccessTokenType.Reference,
                    AccessTokenLifetime = 36000, //10 hours
                    IdentityTokenLifetime = 36000,
                    RequireConsent = false,
                    RequireClientSecret = false,
                    AlwaysSendClientClaims = true,
                    AllowedGrantTypes = GrantTypes.Code,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 36000,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    //EnableLocalLogin = true,
                    IdentityProviderRestrictions = new List<string>
                    {
                        "Facebook",
                        "Google"
                    },
                    RedirectUris = configuration.RedirectUris,
                    PostLogoutRedirectUris = configuration.PostLogoutRedirectUris,
                    AllowedCorsOrigins = configuration.AllowedOrigins,
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        "openid", "profile", "email", "tokenizerapi.full", "roles", "offline_access", IdentityServerConstants.LocalApi.ScopeName
                    }
                },
            };
        }

       
    }
}