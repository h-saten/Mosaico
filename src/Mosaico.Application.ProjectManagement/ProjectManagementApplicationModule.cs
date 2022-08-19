using System;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Application.ProjectManagement.BackgroundJobs;
using Mosaico.Application.ProjectManagement.BackgroundJobs.Crowdsale;
using Mosaico.Application.ProjectManagement.EventHandlers;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.BackgroundJobs.Hangfire.Extensions;
using Mosaico.Domain.ProjectManagement;
using Mosaico.Events.Base;
using Mosaico.SDK.Wallet;
using Mosaico.SDK.BusinessManagement;
using Mosaico.Application.ProjectManagement.CounterProviders;
using Mosaico.Application.ProjectManagement.EventHandlers.Affiliation;
using Mosaico.Core.Abstractions;
using Mosaico.SDK.Features;
using Mosaico.Validation.Base.Extensions;

namespace Mosaico.Application.ProjectManagement
{
    public class ProjectManagementApplicationModule : Module
    {
        public static readonly Type MappingProfileType = typeof(ProjectManagementMapperProfile);
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterMediatR(ThisAssembly);
            //modules
            builder.RegisterModule<ProjectManagementModule>();
            builder.RegisterModule<WalletSDKModule>();
            builder.RegisterModule<BusinessManagementSDKModule>();
            builder.RegisterModule<FeaturesSDKModule>();
            //validators
            builder.RegisterAssemblyValidators(ThisAssembly);
            //event handlers
            builder.RegisterType<SendInvitationOnProjectMemberAdded>().As<IEventHandler>();
            //builder.RegisterType<DeployContractsOnProjectApproved>().As<IEventHandler>();
            builder.RegisterType<UpdateUserPermissionOnMembershipUpdated>().As<IEventHandler>();
            //builder.RegisterType<ScheduleNextStageOnStageFinalized>().As<IEventHandler>();
            builder.RegisterType<DeleteUserPermissionsOnMemberDeleted>().As<IEventHandler>();
            builder.RegisterType<UpdateProjectLogoUrlOnFileUploaded>().As<IEventHandler>();
            builder.RegisterType<AssignTokenOnCreated>().As<IEventHandler>();
            builder.RegisterType<DeleteUserFromProjectTeamMembersOnUserDeletedEvent>().As<IEventHandler>();
            builder.RegisterType<UpdateDocumentsOnConverted>().As<IEventHandler>();
            builder.RegisterType<UpdatePageOnCoverUploaded>().As<IEventHandler>();
            builder.RegisterType<UpdateCountersOnProjectCreated>().As<IEventHandler>();
            builder.RegisterType<UpdateInvestmentPackageOnLogoUploaded>().As<IEventHandler>();
            builder.RegisterType<SendInvestorCertificateOnTransactionFinalized>().As<IEventHandler>();
            builder.RegisterType<AddAffiliationTransactionOnConfirmed>().As<IEventHandler>();
            //counters
            builder.RegisterType<ProjectCounterProvider>().As<ICounterProvider>().AsSelf();
            builder.RegisterType<InvitationCounterProvider>().As<ICounterProvider>().AsSelf();
            //jobs
            builder.RegisterHangfireJob<StageActivationJob>();
            builder.RegisterType<CrowdsaleDeploymentJob>().As<ICrowdsaleDeploymentJob>();
            builder.RegisterType<StageDeploymentJob>().As<IStageDeploymentJob>();
            //other
            builder.RegisterType<ProjectPermissionFactory>().As<IProjectPermissionFactory>();
            builder.RegisterType<ProjectEmailSender>().As<IProjectEmailSender>();
            builder.RegisterType<TokenPageService>().As<ITokenPageService>();
            builder.RegisterType<ProjectDTOAggregatorService>().As<IProjectDTOAggregatorService>();
            builder.RegisterType<AffiliationEmailService>().As<IAffiliationEmailService>();
            
            builder.RegisterType<StageService>().As<IStageService>();
            builder.RegisterType<CertificateGeneratorService>().As<ICertificateGeneratorService>();
            builder.RegisterType<AirdropImportService>().As<IAirdropImportService>();
            builder.RegisterType<UserAffiliationService>().As<IUserAffiliationService>();
        }
    }
}