using System;
using System.Security.Cryptography.X509Certificates;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Mosaico.Application.Identity.Services;
using Mosaico.Core.EntityFramework.Configurations;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Identity.Configurations;
using Mosaico.Persistence.SqlServer.Contexts.Identity;
using Mosaico.Persistence.SqlServer.Contexts.PersistedGrant;
using static Mosaico.API.Base.Constants.Permissions;

namespace Mosaico.Identity.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddMosaicoIdentityServer(this IServiceCollection services, SqlServerConfiguration sqlConfig, IdentityServiceConfiguration serviceConfig, X509Certificate2 cert)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(config => {
                    config.Password.RequireNonAlphanumeric = false;
                    config.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddLocalApiAuthentication();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
                {
                    policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy(AppServicesInternalActions, policy =>
                {
                    policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", AppServicesInternalActions);
                });
            });
            
            var builder = services
                .AddIdentityServer(options =>
                {
                    var issueUri = serviceConfig.IssuerUri;
                    if (!string.IsNullOrEmpty(issueUri))
                    {
                        options.IssuerUri = issueUri;
                        Console.WriteLine($"Using IssuerUri:{issueUri}");
                    }
                    else
                    {
                        Console.WriteLine($"No IssuerUri was set");
                    }

                    options.UserInteraction.LoginUrl = "/auth/login";
                    options.UserInteraction.LogoutUrl = "/auth/logout";
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    
                })
                .AddOperationalStore<IdentityPersistedGrantDbContext>()
                .AddSigningCredential(cert)
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis(serviceConfig))
                .AddInMemoryClients(Config.GetClients(serviceConfig))
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityProfileService>();
        }
    }
}