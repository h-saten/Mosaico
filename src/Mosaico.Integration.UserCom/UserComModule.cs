using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Integration.UserCom.Abstractions;
using Mosaico.Integration.UserCom.Configurations;

namespace Mosaico.Integration.UserCom
{
    /*
     * Module which contains registrations of User.com API - User.com integration
     */
    public class UserComModule : Module
    {
        private readonly UserComConfig _config = new();

        public UserComModule(IConfiguration configuration)
        {
            configuration.GetSection(UserComConfig.SectionName).Bind(_config);
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_config).AsSelf();
            builder.RegisterType<ConversationsClient>().As<IUserComConversationsClient>();
            builder.RegisterType<UsersClient>().As<IUserComUsersApiClient>();
        }
    }
}