using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Application.Identity.EventHandlers;
using Mosaico.Events.Base;

namespace Mosaico.Application.Identity
{
    public class IdentityEventModule : Module
    {
        private readonly IConfiguration _configuration;

        public IdentityEventModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //builder.RegisterType<UpdateEventStorageOnUserEvents>().As<IEventHandler>();
            builder.RegisterType<SendForgotPasswordEmailOnResetInitiated>().As<IEventHandler>();

            builder.RegisterType<SendEmailChangeEmailOnChangeInitiated>().As<IEventHandler>();
            builder.RegisterType<SendEmailChangeConfirmation>().As<IEventHandler>();
            builder.RegisterType<SendPasswordChangeConfirmation>().As<IEventHandler>();
            builder.RegisterType<SendPhoneNumberChangeConfirmation>().As<IEventHandler>();

            builder.RegisterType<SendChangePasswordEmailInitiated>().As<IEventHandler>();
            builder.RegisterType<SendEmailOnUserDeletionRequest>().As<IEventHandler>();
            builder.RegisterType<SendResetPasswordEmailOnUserRestore>().As<IEventHandler>();
            builder.RegisterType<SendRegisterConfirmationEmailOnUserCreated>().As<IEventHandler>();
            builder.RegisterType<SendRegisterConfirmationEmailOnExternalUserCreated>().As<IEventHandler>();
            builder.RegisterType<UpdateLastLoginOnUserLoggedIn>().As<IEventHandler>();
            builder.RegisterType<RemoveUserPermissionsOnRequested>().As<IEventHandler>();
            builder.RegisterType<AddUserPermissionsOnRequested>().As<IEventHandler>();
            builder.RegisterType<UpdateUserPhotoOnFileUploaded>().As<IEventHandler>();
            builder.RegisterType<RemoveUserPhotoOnFileDeleted>().As<IEventHandler>();
            builder.RegisterType<VerifyUserOnPaymentSucceeded>().As<IEventHandler>();
        }
    }
}