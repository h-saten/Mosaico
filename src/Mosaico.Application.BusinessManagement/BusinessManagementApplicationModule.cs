using System;
using Autofac;
using FluentValidation;
using MediatR.Extensions.Autofac.DependencyInjection;
using Mosaico.Application.BusinessManagement.Abstractions;
using Mosaico.Application.BusinessManagement.Commands.AcceptInvitation;
using Mosaico.Application.BusinessManagement.Commands.CreateCompany;
using Mosaico.Application.BusinessManagement.Commands.CreateCompanyTeamMember;
using Mosaico.Application.BusinessManagement.Commands.CreateVerification;
using Mosaico.Application.BusinessManagement.Commands.DeleteCompanyTeamMember;
using Mosaico.Application.BusinessManagement.Commands.LeaveCompany;
using Mosaico.Application.BusinessManagement.Commands.ResendInvitation;
using Mosaico.Application.BusinessManagement.Commands.Subscribe;
using Mosaico.Application.BusinessManagement.Commands.Unsubscribe;
using Mosaico.Application.BusinessManagement.Commands.UpdateCompany;
using Mosaico.Application.BusinessManagement.Commands.UpdateInvitation;
using Mosaico.Application.BusinessManagement.CounterProviders;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Application.BusinessManagement.EventHandlers;
using Mosaico.Application.BusinessManagement.Permissions;
using Mosaico.Application.BusinessManagement.Queries.CompanyPermissions;
using Mosaico.Application.BusinessManagement.Queries.GetAllVerifications;
using Mosaico.Application.BusinessManagement.Queries.GetCompanies;
using Mosaico.Application.BusinessManagement.Queries.GetCompany;
using Mosaico.Application.BusinessManagement.Queries.GetCompanyTeamMembers;
using Mosaico.Application.BusinessManagement.Queries.GetInvitation;
using Mosaico.Application.BusinessManagement.Queries.GetInvitations;
using Mosaico.Application.BusinessManagement.Queries.GetVerification;
using Mosaico.Application.BusinessManagement.Services;
using Mosaico.Core.Abstractions;
using Mosaico.Domain.BusinessManagement;
using Mosaico.Events.Base;
using Mosaico.SDK.DocumentManagement;
using Mosaico.SDK.Features;
using Mosaico.SDK.Wallet;
using Mosaico.Validation.Base.Extensions;

namespace Mosaico.Application.BusinessManagement
{
    public class BusinessManagementApplicationModule : Module
    {
        public static readonly Type MappingProfileType = typeof(BusinessManagementMapperProfile);
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterMediatR(ThisAssembly);
            //modules
            builder.RegisterModule<BusinessManagementModule>();
            builder.RegisterModule<DocumentManagementSDKModule>();
            builder.RegisterModule<FeaturesSDKModule>();
            //validators
            builder.RegisterAssemblyValidators(ThisAssembly);

            //event handlers
            builder.RegisterType<DeleteUserPermissionsOnMemberDeleted>().As<IEventHandler>();
            builder.RegisterType<UpdateUserPermissionOnMembershipUpdated>().As<IEventHandler>();
            builder.RegisterType<SendEmailRequestsOnCompanyVerification>().As<IEventHandler>();
            builder.RegisterType<DeleteUserMembershipOnUserDeletedEvent>().As<IEventHandler>();
            builder.RegisterType<UpdateCompanyLogoOnUploaded>().As<IEventHandler>();
            builder.RegisterType<UpdateCountersOnCompanyCreated>().As<IEventHandler>();
            builder.RegisterType<CreateProposalOnBlockchain>().As<IEventHandler>();
            builder.RegisterType<VoteOnBlockchain>().As<IEventHandler>();
            //other
            builder.RegisterType<CompanyPermissionFactory>().As<ICompanyPermissionFactory>();
            //counters
            builder.RegisterType<CompanyCounterProvider>().As<ICounterProvider>().AsSelf();
            //email sender
            builder.RegisterType<CompanyEmailSender>().As<ICompanyEmailSender>();
            //services
            builder.RegisterType<ProposalService>().As<IProposalService>();
        }
    }
}