using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<TContext, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterDbContext<TContext, TFactory>(this ContainerBuilder builder, string connectionString) where TContext : DbContext where TFactory : IDbFactory<TContext>, new()
        {
            builder.Register(componentContext =>
            {
                var serviceProvider = componentContext.Resolve<IServiceProvider>();
                
                var factory = new TFactory();
                factory.SetConnectionString(connectionString);
                var dbContextOptions = new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>());
                var optionsBuilder = factory.GetOptions(dbContextOptions);
                optionsBuilder.UseApplicationServiceProvider(serviceProvider);

                return optionsBuilder.Options;
            }).As<DbContextOptions<TContext>>().InstancePerLifetimeScope();

            return builder.RegisterType<TContext>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}