using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.API.Base;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.v1.DocumentManagement;
using Mosaico.API.v1.ProjectManagement;
using Mosaico.API.v1.Wallet;
using Mosaico.API.v1.BusinessManagement;
using Mosaico.API.v1.Core;
using Mosaico.API.v1.Statistics;
using Mosaico.BackgroundJobs.Hangfire;
using Mosaico.Core.ResourceManager;
using Mosaico.Core.Service.Configurations;
using Mosaico.DocumentExport.CKEditor;
using Mosaico.DocumentExport.PDF;
using Mosaico.Integration.Blockchain.CoinAPI;
using Mosaico.Integration.Email.EmailLabs;
using Mosaico.Integration.Email.Local;
using Mosaico.Integration.SignalR;
using Mosaico.Integration.Sms.Local;
using Mosaico.Integration.Sms.SmsLabs;
using Mosaico.Integration.UserCom;
using Mosaico.Persistence.SqlServer;
using Mosaico.SDK.Identity;
using Mosaico.SDK.Identity.Configurations;
using Mosaico.Statistics.GoogleAnalytics;
using Mosaico.Statistics.Local;
using Mosaico.SDK.BusinessManagement;
using Mosaico.SDK.ProjectManagement;
using Mosaico.Integration.Email.SendGridEmail;
using Mosaico.SDK.Relay;

namespace Mosaico.Core.Service
{
    public class ServiceModule : Module
    {
        private readonly IConfiguration _configuration;

        public ServiceModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
    
            var mappingTypes = new List<Type>
            {
                ProjectManagementAPIv1Module.MappingProfileType,
                BusinessManagementAPIv1Module.MappingProfileType,
                DocumentManagementAPIv1Module.MappingProfileType,
                StatisticsAPIv1Module.MappingProfileType,
                BusinessManagementSDKModule.MappingProfileType,
                ProjectManagementSDKModule.MappingProfileType
            };
            mappingTypes.AddRange(WalletAPIv1Module.MappingProfileType);

            var serviceConfig = new ServiceConfiguration();
            _configuration.GetSection(ServiceConfiguration.SectionName).Bind(serviceConfig);
            builder.RegisterInstance(serviceConfig);

            builder.RegisterModule(new CoreModule(mappingTypes, _configuration));
            builder.RegisterModule(new CoreAPIModule());
            builder.RegisterModule(new DomainModule(_configuration));
            builder.RegisterModule(new IdentityDomainModule(_configuration));
            builder.RegisterModule(new ProjectManagementAPIv1Module());
            builder.RegisterModule(new BusinessManagementAPIv1Module());
            builder.RegisterModule(new WalletAPIv1Module(_configuration));
            builder.RegisterModule(new DocumentManagementAPIv1Module(_configuration));
            builder.RegisterModule(new CKEditorModule(_configuration));
            builder.RegisterModule(new PdfModule(_configuration));
            builder.RegisterModule(new CoinApiModule(_configuration));
            builder.RegisterModule(new StatisticsAPIv1Module());
            builder.RegisterModule(new RelayModule(_configuration));
            
            switch (serviceConfig.AuthType)
            {
                case "IdentityServer":
                    var identityServerConfiguration = new IdentityServerConfiguration();
                    _configuration.GetSection(IdentityServerConfiguration.SectionName).Bind(identityServerConfiguration);
                    builder.RegisterModule(new IdentitySDKModule(identityServerConfiguration));
                    builder.RegisterModule(new AuthorizationModule());
                    break;
                default:
                    throw new UnsupportedModuleException(ServiceConfiguration.SectionName);
            }
        
            builder.RegisterModule(new HangfireModule(_configuration));
            builder.RegisterModule<ResourceManagerModule>();
            builder.RegisterModule<SignalRModule>();
            builder.RegisterModule(new UserComModule(_configuration));
            builder.RegisterModule(new GoogleAnalyticsModule(_configuration));
            LoadEmailProvider(builder, serviceConfig);
            LoadSmsProvider(builder, serviceConfig);
            LoadAnalyticsProvider(builder, serviceConfig);
        }

        private void LoadEmailProvider(ContainerBuilder builder, ServiceConfiguration serviceConfig)
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

        private void LoadSmsProvider(ContainerBuilder builder, ServiceConfiguration serviceConfig)
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

        private void LoadAnalyticsProvider(ContainerBuilder builder, ServiceConfiguration serviceConfig)
        {
            switch (serviceConfig.AnalyticsProvider)
            {
                case "GoogleAnalytics":
                    builder.RegisterModule(new GoogleAnalyticsModule(_configuration));
                    break;
                case "Local":
                    builder.RegisterModule<LocalAnalyticsModule>();
                    break;
                default:
                    throw new UnsupportedModuleException("AnalyticsProvider");
            }
        }
    }
}