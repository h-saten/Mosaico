using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Mosaico.Authorization.Base;
using Mosaico.Core.EntityFramework;
using NUnit.Framework;

namespace Mosaico.Tests.Base
{
    public abstract class EFInMemoryTestBase : MediatrTestBase
    {
        protected ICurrentUserContext CurrentUserContext;
        protected abstract List<Profile> Profiles { get; }
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            if (Profiles != null)
            {
                var mapper = new Mapper(new MapperConfiguration(m =>
                    m.AddProfiles(Profiles))
                );
                builder.RegisterInstance(mapper).AsImplementedInterfaces();
            }
        }

        protected IMapper Mapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfiles(Profiles));
            return config.CreateMapper();
        }
        
        protected TContext RegisterContext<TContext>(ContainerBuilder builder) where TContext : DbContext, IDbContext
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<TContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var options = dbContextOptionsBuilder.Options;
            var context = Activator.CreateInstance(typeof(TContext), options, null);
            builder.RegisterInstance(context).AsSelf().AsImplementedInterfaces().As<IDbContext>();
            return (TContext) context;
        }

        protected ICurrentUserContext CreateCurrentUserContextMock()
        {
            var mock = new Mock<ICurrentUserContext>();
            mock.Setup(c => c.UserId).Returns(Guid.NewGuid().ToString());
            mock.Setup(c => c.Email).Returns("test@mosaico.ai");
            return mock.Object;
        }

        public TContext GetContext<TContext>() where TContext : class, IDbContext
        {
            var context = Container.Resolve<TContext>();
            return context;
        }

        [TearDown]
        public void EFTearDown()
        {
            var contexts = Container.Resolve<IEnumerable<IDbContext>>();
            foreach (var dbContext in contexts)
            {
                DeleteAndDisposeDbContext(dbContext);
            }
        }

        private void DeleteAndDisposeDbContext(IDbContext dbContext)
        {
            MethodInfo method = typeof(EFInMemoryTestBase).GetMethod(nameof(GetContext));
            MethodInfo generic = method.MakeGenericMethod(dbContext.GetType());
            var context = (DbContext) generic.Invoke(this, null);
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [SetUp]
        public void EFSetup()
        {
            var contexts = Container.Resolve<IEnumerable<IDbContext>>();
            foreach (var dbContext in contexts)
            {
                MethodInfo method = typeof(EFInMemoryTestBase).GetMethod(nameof(GetContext));
                MethodInfo generic = method.MakeGenericMethod(dbContext.GetType());
                var context = (DbContext) generic.Invoke(this, null);
                context.Database.EnsureCreated();
            }
        }
    }
}