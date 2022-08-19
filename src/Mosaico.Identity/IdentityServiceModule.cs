using System;
using System.Collections.Generic;
using Autofac;
using KangaExchange.SDK;
using Microsoft.Extensions.Configuration;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.v1.Identity;
using Mosaico.Authorization.Base;
using Mosaico.Authorization.Base.Configurations;
using Mosaico.BackgroundJobs.Hangfire;
using Mosaico.Base;
using Mosaico.Base.Extensions;
using Mosaico.Core;
using Mosaico.Core.ResourceManager;
using Mosaico.Identity.Configurations;
using Mosaico.Integration.Email.EmailLabs;
using Mosaico.Integration.Email.Local;
using Mosaico.Integration.Email.SendGridEmail;
using Mosaico.Integration.Sms.Local;
using Mosaico.Integration.Sms.SmsLabs;
using Mosaico.KYC.Passbase;
using Mosaico.Persistence.SqlServer;

namespace Mosaico.Identity
{
    public class IdentityServiceModule : Module
    {
        private readonly IConfiguration _configuration;

        public IdentityServiceModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var serviceConfig = new IdentityServiceConfiguration();
            _configuration.GetSection(IdentityServiceConfiguration.SectionName).Bind(serviceConfig);
            
            var authenticatorConfig = new AuthenticatorConfiguration();
            _configuration.GetSection(AuthenticatorConfiguration.SectionName).Bind(authenticatorConfig);

            builder.AddUrl(serviceConfig.BaseUri, Application.Identity.Constants.UrlKeys.BaseUri);
            builder.AddUrl(serviceConfig.AfterLoginRedirectUrl, Application.Identity.Constants.UrlKeys.AfterLoginRedirectUrl);
            
            builder.RegisterInstance(serviceConfig).AsSelf();
            builder.RegisterInstance(authenticatorConfig).AsSelf();
                            
            var mappingTypes = new List<Type>
            {
                IdentityAPIv1Module.MappingProfileType
            };
            builder.RegisterModule(new HangfireModule(_configuration));
            builder.RegisterModule(new CoreModule(mappingTypes, _configuration));
            builder.RegisterModule(new IdentityDomainModule(_configuration));
            builder.RegisterModule(new IdentityAPIv1Module(_configuration));
            builder.RegisterModule(new KangaModule(_configuration));
            builder.RegisterModule<ResourceManagerModule>();
            builder.RegisterModule(new CaptchaModule(_configuration));
            builder.RegisterModule(new PassbaseModule(_configuration));
            //builder.RegisterModule(new MongoDbDominModule(_configuration));
            LoadEmailProvider(builder, serviceConfig);
            LoadSmsProvider(builder, serviceConfig);
            
            builder.RegisterType<CurrentUserContext>().As<ICurrentUserContext>().InstancePerLifetimeScope();
        }
        
        private void LoadEmailProvider(ContainerBuilder builder, IdentityServiceConfiguration serviceConfig)
        {
            switch (serviceConfig.EmailProvider)
            {
                case "EmailLabs":
                    builder.RegisterModule(new EmailLabsModule(_configuration));
                    break;
                case "Local":
                    builder.RegisterModule<LocalEmailSenderModule>();
                    break;
                case "SendGridEmail":
                    builder.RegisterModule(new SendGridEmailModule(_configuration));
                    break;
                default:
                    throw new UnsupportedModuleException("EmailProvider");
            }
        }

        private void LoadSmsProvider(ContainerBuilder builder, IdentityServiceConfiguration serviceConfig)
        {
            switch (serviceConfig.SmsProvider)
            {
                case "SmsLabs":
                    builder.RegisterModule(new SmsLabsModule(_configuration));
                    break;
                case "Local":
                    builder.RegisterModule<LocalSmsSenderModule>();
                    break;
                default:
                    throw new UnsupportedModuleException("SmsProvider");
            }
        }
    }
}