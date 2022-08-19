using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.BusinessManagement.Commands.CreateCompany;
using Mosaico.Domain.BusinessManagement;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.BusinessContext;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.BusinessManagement.Tests
{
    public class CreateBusinessCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;

        private Mock<IUserManagementClient> _managementClientMock;

        protected override List<Profile> Profiles => new List<Profile>
        {
            new BusinessManagementMapperProfile()
        };

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            
            RegisterContext<BusinessContext>(builder);
            builder.RegisterModule<BusinessManagementModule>();
            builder.RegisterModule(new BusinessManagementApplicationModule());
            
            _eventPublisherMock = new Mock<IEventPublisher>();
            _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
            builder.RegisterInstance(_eventPublisherMock.Object).AsImplementedInterfaces();

            _managementClientMock = new Mock<IUserManagementClient>();
            builder.RegisterInstance(_managementClientMock.Object).As<IUserManagementClient>();

            _eventFactory = new CloudEventFactory();
            builder.RegisterInstance(_eventFactory).As<IEventFactory>();
        }

        [Test]
        public async Task ShouldCreateCompany()
        {
            // Arrange
            var command = new CreateCompanyCommand
            {
                CompanyName = "Test Company",
                Country = "Test Country",
                Street = "Test Street",
                VATId = "Test VATId",
                PostalCode = "Test PostalCode"
            };
            // Act
            var id = await SendAsync(command);
            
            // Assert
            id.CompanyId.Should().NotBe(Guid.Empty);
            
            var context = GetContext<BusinessContext>();
            var company = await context.Companies.FirstOrDefaultAsync(u => u.Id == id.CompanyId);

            company.Should().NotBeNull();
            company.CompanyName.Should().Be("Test Company");
            company.Country.Should().Be("Test Country");
            company.Street.Should().Be("Test Street");
            company.TeamMembers.Should().NotBeEmpty();
            company.VATId.Should().Be("Test VATId");
            company.PostalCode.Should().Be("Test PostalCode");
        
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);
        }
        
        [Test]
        public async Task ShouldNotCreateCompanyWithoutName()
        {
            // Arrange
            var command = new CreateCompanyCommand
            {
                Country = "Test Country",
                Street = "Test Street",
                VATId = "Test VATId",
                PostalCode = "Test PostalCode"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<CompanyMustHaveAName>();
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }
        
        
        
        [Test]
        public async Task ShouldFailWhenDuplicateCompanyCreationAttempt()
        {
            // Arrange
            var command = new CreateCompanyCommand
            {
                CompanyName = "Test Company",
                Country = "Test Country",
                Street = "Test Street",
                VATId = "Test VATId",
                PostalCode = "Test PostalCode"
            };
            await SendAsync(command);
            var command2 = new CreateCompanyCommand
            {
                CompanyName = "Test Company",
                Country = "Test Country",
                Street = "Test Street",
                VATId = "Test VATId",
                PostalCode = "Test PostalCode"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command2);
            };
            await action.Should().ThrowAsync<CompanyAlreadyExistsException>();
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);
        }
    }
}