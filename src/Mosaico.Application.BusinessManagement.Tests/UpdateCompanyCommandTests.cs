using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.BusinessManagement.Commands.UpdateCompany;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.BusinessContext;
using Mosaico.Tests.Base;
using NUnit.Framework;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.BusinessManagement.Tests
{
    [TestFixture]
    public class UpdateCompanyCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;

        protected override List<Profile> Profiles => new List<Profile>
        {
            new BusinessManagementMapperProfile()
        };
        
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            _eventFactory = new CloudEventFactory();
            builder.RegisterInstance(_eventFactory).As<IEventFactory>();
            RegisterContext<BusinessContext>(builder);
            _eventPublisherMock = new Mock<IEventPublisher>();
            _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
            builder.RegisterInstance(_eventPublisherMock.Object).As<IEventPublisher>();

            builder.RegisterType<UpdateCompanyCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<UpdateCompanyCommandValidator>().As<IValidator<UpdateCompanyCommand>>();
        }
        
        private async Task<Company> InitiateCompany(BusinessContext context)
        {
            var company = new Company
            {
                Id = Guid.NewGuid(),
                CompanyName = "Test Company",
                Country = "Test Country",
                PostalCode = "Test Postal",
                Size = "S",
                Street = "Test Street",
                VATId = "Test VATId"
            };
            context.Add(company);
            await context.SaveChangesAsync();
            return company;
        }

        [Test]
        public async Task ShouldUpdateCompany()
        {
            // Arrange
            var context = GetContext<BusinessContext>();
            var company = await InitiateCompany(context);
            var command = new UpdateCompanyCommand
            {
                CompanyId = company.Id,
                Country = "Test Country 2",
                PostalCode = "Test Postal 2",
                Size = "S",
                Street = "Test Street 2",
                VATId = "Test VATId 2",
                PhoneNumber = "576560090",
                Email = "greg@mosaico.ai"
            };
            
            // Act
            var response = await SendAsync(command);
            
            // Assert
            var dbBusiness = await context.Companies.FirstOrDefaultAsync(p => p.Id == company.Id);
            dbBusiness.Should().NotBeNull();
            dbBusiness.CompanyName.Should().Be("Test Company 2");
            dbBusiness.Size.Should().Be("S");
            dbBusiness.PostalCode.Should().Be("Test Postal 2");
            dbBusiness.Street.Should().Be("Test Street 2");
            dbBusiness.VATId.Should().Be("Test VATId 2");
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);


        }

        [Test]
        public async Task ShouldFailIfCompanyNameIsEmpty()
        {
            // Arrange
            var context = GetContext<BusinessContext>();
            var company = await InitiateCompany(context);
            var command = new UpdateCompanyCommand
            {
                Country = "Test Country 2",
                PostalCode = "Test Postal 2",
                Size = "S",
                Street = "Test Street 2",
                VATId = "Test VATId 2",
                PhoneNumber = "576560090",
                Email = "greg@mosaico.ai"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task ShouldFailIfCompanyIdEmpty()
        {
            // Arrange
            var context = GetContext<BusinessContext>();
            var project = await InitiateCompany(context);
            var command = new UpdateCompanyCommand
            {
                Country = "Test Country 2",
                PostalCode = "Test Postal 2",
                Size = "S",
                Street = "Test Street 2",
                VATId = "Test VATId 2",
                PhoneNumber = "576560090",
                Email = "greg@mosaico.ai"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<CompanyNotFoundException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status404NotFound);
        }
        
        [Test]
        public async Task ShouldFailIfCompanyNotFound()
        {
            // Arrange
            var context = GetContext<BusinessContext>();
            var company = await InitiateCompany(context);
            var command = new UpdateCompanyCommand
            {
                CompanyId = Guid.NewGuid(),
                Country = "Test Country 2",
                PostalCode = "Test Postal 2",
                Size = "S",
                Street = "Test Street 2",
                VATId = "Test VATId 2",
                PhoneNumber = "576560090",
                Email = "greg@mosaico.ai"
            };

            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<CompanyNotFoundException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status404NotFound);
        }
    }
}