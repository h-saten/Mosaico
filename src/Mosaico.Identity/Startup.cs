using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AspNetCoreRateLimit;
using Autofac;
using MassTransit;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mosaico.API.Base.Extensions;
using Mosaico.API.Base.Filters;
using Mosaico.Authorization.Base.Configurations;
using Mosaico.BackgroundJobs.Hangfire.Configurations;
using Mosaico.BackgroundJobs.Hangfire.Extensions;
using Mosaico.Base.Abstractions;
using Mosaico.Base.Extensions;
using Mosaico.Base.Settings;
using Mosaico.Core.EntityFramework.Configurations;
using Mosaico.Identity.Configurations;
using Mosaico.Identity.Extensions;
using Mosaico.Identity.Filters;
using Mosaico.Secrets.KeyVault.Configurations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;

namespace Mosaico.Identity
{
    public class Startup
    {
        private readonly HangfireConfig _hangfireConfiguration = new();
        private readonly IdentityServiceConfiguration _serviceConfiguration = new();
        private readonly SqlServerConfiguration _sqlServerConfiguration = new();
        private readonly KeyVaultConfiguration _keyVaultConfiguration = new();
        private readonly LoggersSetting _loggersSettings = new();
        private readonly ExternalProvidersConfiguration _externalProviders = new();
        private readonly IdentityServiceConfiguration _identityService = new();
        private readonly AuthenticationConfiguration _authenticationConfiguration = new();

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _environment;
        public IHostEnvironment Environment { get; }
        
        public Startup(IConfiguration configuration, IHostEnvironment environment, IWebHostEnvironment environment1)
        {
            Configuration = configuration;
            Environment = environment;
            _environment = environment1;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.GetSection(HangfireConfig.SectionName).Bind(_hangfireConfiguration);
            Configuration.GetSection(IdentityServiceConfiguration.SectionName).Bind(_serviceConfiguration);
            Configuration.GetSection(Domain.Identity.Constants.IdentityServerDbConfigurationSection).Bind(_sqlServerConfiguration);
            Configuration.GetSection(KeyVaultConfiguration.SectionName).Bind(_keyVaultConfiguration);
            Configuration.GetSection(LoggersSetting.SectionName).Bind(_loggersSettings);
            Configuration.GetSection(ExternalProvidersConfiguration.SectionName).Bind(_externalProviders);
            Configuration.GetSection(IdentityServiceConfiguration.SectionName).Bind(_identityService);
            Configuration.GetSection(AuthenticationConfiguration.SectionName).Bind(_authenticationConfiguration);
            
            _hangfireConfiguration.ConnectionString = _sqlServerConfiguration.ConnectionString;
            services.AddHangfire(_hangfireConfiguration);

            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddHttpContextAccessor();
            
            var certificate = services.GetCertificateAsync(_environment, _serviceConfiguration.Certificate, _keyVaultConfiguration).GetAwaiter().GetResult();
            services.AddDataProtection(_sqlServerConfiguration.ConnectionString, "identity", certificate);

            services.AddVersionedAPIWithSwagger();
            
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("pl-PL"),
                            new CultureInfo("en-EN")
                        };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-EN", uiCulture: "en-EN");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.FallBackToParentCultures = false;
                    options.FallBackToParentUICultures = false;

                    var providerQuery = new LocalizationQueryProvider
                    {
                        QueryParameterName = "ui_locales"
                    };

                    options.RequestCultureProviders.Insert(0, providerQuery);
                });

            services.AddMosaicoIdentityServer(_sqlServerConfiguration, _serviceConfiguration, certificate);
            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add<CustomExceptionFilterAttribute>();
                    options.Filters.Add<SecurityHeadersAttribute>();
                })
                .AddViewLocalization()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            var cors = _serviceConfiguration
                .AllowedOrigins?
                .AsEnumerable()
                .Where(m => !string.IsNullOrEmpty(m))
                .Select(m => m)
                .ToArray();

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(cors)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = _environment.IsDevelopment() ? "ClientApp/dist" : "ClientApp";
            });
            
            var authBuilder = services.AddAuthentication();
            
            if (_externalProviders?.Facebook?.IsEnabled == true)
            {
                authBuilder.AddCustomFacebook(
                    _externalProviders.Facebook, 
                    _externalProviders.ErrorRedirectUrl, 
                    _identityService.BaseUri);
            }

            if (_externalProviders?.Google?.IsEnabled == true)
            {
                authBuilder.AddCustomGoogle(
                    _externalProviders.Google, 
                    _externalProviders.ErrorRedirectUrl, 
                    _identityService.BaseUri);
            }
            services.AddMassTransitHostedService(true);      

            services.AddRateLimit(Configuration);
            services.AddSingleton<ITelemetryInitializer, IdentityTelemetryInitializer>();
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInstance(Configuration).AsImplementedInterfaces();

            var logger = new LoggerConfiguration()
                .UseConsoleLogger(_loggersSettings.ConsoleLogger)
                .UseFileLogger(_loggersSettings.FileLogger)
                .UseApplicationInsights(_loggersSettings.ApplicationInsightsLogger)
                .MinimumLevel.Verbose()
                .CreateLogger();

            Log.Logger = logger;
            builder.RegisterInstance(logger).As<ILogger>();
            builder.RegisterInstance(_externalProviders);
            builder.RegisterInstance(_serviceConfiguration);
            builder.RegisterInstance(_authenticationConfiguration);
            builder.RegisterModule(new IdentityServiceModule(Configuration));
        }
        
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IApiVersionDescriptionProvider provider, IMigrationRunner runner)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
            }

            if (_serviceConfiguration.SwaggerEnabled)
            {
                // http://localhost:10000/id/swagger
                // http://localhost:4333/id/swagger
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "id/swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(
                    options =>
                    {
                        // build a swagger endpoint for each discovered API version
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"/id/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                            options.RoutePrefix = "id/swagger";
                        }
                    });
            }
            
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
                RequireHeaderSymmetry = false
            };
            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            // ref: https://github.com/aspnet/Docs/issues/2384
            app.UseForwardedHeaders(forwardOptions);

            app.UseHangfire(_hangfireConfiguration);
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });
            
            

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            var fileExtensionProvider = new FileExtensionContentTypeProvider();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = fileExtensionProvider // this is not set by default
            });

            app.UseSpaStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = fileExtensionProvider // this is not set by default
            });
            app.UseIdentityServer();

            app.UseIpRateLimiting();

            app.UseCors("default");
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                if (!env.IsDevelopment())
                {
                    spa.Options.SourcePath = "ClientApp";
                }
                if (env.IsDevelopment())
                {
                    spa.Options.SourcePath = "../../frontend/mosaico-id-ui";
                    spa.UseProxyToSpaDevelopmentServer(_serviceConfiguration.SpaURL);
                }
            });
            
            if (_serviceConfiguration.RunMigrations)
            {
                if (runner == null) throw new Exception("MigrationRunner was not registered");
                runner.RunMigrations();
            }
            
            SeedData.EnsureSeedData(app.ApplicationServices, _environment);
        }
    }
}
