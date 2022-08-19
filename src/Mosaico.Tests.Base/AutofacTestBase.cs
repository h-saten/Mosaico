using System.Collections.Generic;
using Autofac;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Tests.Base
{
    public abstract class AutofacTestBase
    {
        protected IContainer Container;

        protected virtual void RegisterDependencies(ContainerBuilder builder)
        {
            var loggerMock = new Mock<ILogger>().Object;
            builder.RegisterInstance(loggerMock).AsSelf().AsImplementedInterfaces();
        }

        [SetUp]
        public void SetUpDIContainer()
        {
            var builder = new ContainerBuilder();
            RegisterDependencies(builder);
            Container = builder.Build();
        }

        [TearDown]
        public void TearDownDIContainer()
        {
            Container?.Dispose();
        }
    }
}