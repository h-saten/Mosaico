using System.Collections.Generic;
using Autofac;
using AutoMapper;
using Moq;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.ProjectContext;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.Tests.Base;

namespace Mosaico.Application.ProjectManagement.Tests.Services
{
    public class CertificateGeneratorServiceTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;
        private Mock<IUserManagementClient> _managementClient;

        protected override List<Profile> Profiles => new List<Profile>
        {
        };
        
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<ProjectContext>(builder);
            _eventPublisherMock = new Mock<IEventPublisher>();
            _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
            builder.RegisterInstance(_eventPublisherMock.Object).As<IEventPublisher>();
        }
        //
        // public async Task ShouldGeneratePdfWithPassedData()
        // {
        //     var dbContext = GetContext<ProjectContext>();
        //     var sut = new CertificateGeneratorService(dbContext);
        // }
    }
}