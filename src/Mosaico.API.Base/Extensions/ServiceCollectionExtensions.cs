using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Duende.IdentityServer;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Mosaico.API.Base.Utils.Tokens;
using Mosaico.Base;
using Mosaico.Base.Configurations;
using Mosaico.Base.Exceptions;
using Mosaico.Persistence.SqlServer.Contexts.DataProtection;
using Mosaico.SDK.Identity.Configurations;
using Mosaico.Secrets.KeyVault.Certificates;
using Mosaico.Secrets.KeyVault.Configurations;

namespace Mosaico.API.Base.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static Task<X509Certificate2> GetCertificateAsync(this IServiceCollection services, IWebHostEnvironment env, CertificateConfiguration cert, KeyVaultConfiguration keyVault = null)
        {
            if (cert == null)
            {
                return Task.FromResult((X509Certificate2)null);
            }
            
            if (env.IsDevelopment())
            {
                var certificateName = Path.Combine(env.ContentRootPath, cert.FileName);
                var certificateService = new FileCertificateService();
                return certificateService.GetCertificateAsync(certificateName, cert.Password);
            }
            else
            {
                if (keyVault == null)
                {
                    throw new InvalidConfigException(KeyVaultConfiguration.SectionName);
                }

                var certificateService = new KeyVaultCertificateService(keyVault);
                return certificateService.GetCertificateAsync(cert.FileName);
            }
        }
        
        public static void AddDataProtection(this IServiceCollection services, string connectionString, string appName, X509Certificate2 cert)
        {
            services.AddDataProtection()
                .SetApplicationName(appName)
                .PersistKeysToDbContext<DataProtectionDbContext>()
                .ProtectKeysWithCertificate(cert)
                .AddKeyManagementOptions(
                    options =>
                    {
                        var factory = new DataProtectionDbContextFactory();
                        factory.SetConnectionString(connectionString);
                        options.XmlRepository = new SqlXmlRepository(factory.CreateDbContext(null));
                    }
                );
        }
        
        public static void AddIdentityServerAuth(this IServiceCollection services, IdentityServerConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddAuthentication("token")
                .AddJwtBearer("token", options =>
                {
                    options.RequireHttpsMetadata = env.IsProduction();
                    options.Authority = configuration.Authority;
                    options.MapInboundClaims = true;
                    options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidTypes = new[] { "at+jwt" },
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hubs")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                }).AddOAuth2Introspection("introspection", options =>
                {
                    options.Authority = configuration.Authority;
                    options.ClientId = configuration.ClientId;
                    options.ClientSecret = configuration.Secret;
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiCaller", policy =>
                {
                    policy.RequireClaim("scope", configuration.ClientId);
                });
            });
            JwtSecurityTokenHandler.DefaultMapInboundClaims = true;
        }
        
        public static void AddClientIdentityServerAuth(this IServiceCollection services, IdentityServerConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddAccessTokenManagement(options =>
            {
                options.Client.Clients.Add("identityserver", new ClientCredentialsTokenRequest
                {
                    Address = $"{configuration.Authority}/connect/token",
                    ClientId = configuration.Api.ClientId,
                    ClientSecret = configuration.Api.ClientSecret,
                    Scope = configuration.Api.Scopes,
                });
            });
            services.AddClientAccessTokenClient(configuration.Api.ClientName, configureClient: client =>
            {
                client.BaseAddress = new Uri(configuration.Authority);
            });
        }
        
        
    }
}