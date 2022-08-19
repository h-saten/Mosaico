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
using Mosaico.Application.BusinessManagement.Commands.CreateVerification;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Domain.BusinessManagement;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Persistence.SqlServer.Contexts.BusinessContext;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.Tests.Base;
using Mosaico.Validation.Base.Exceptions;
using NUnit.Framework;

namespace Mosaico.Application.BusinessManagement.Tests
{
    public class CreateVerificationCommandTests : EFInMemoryTestBase
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
            
            RegisterContext<BusinessContext>(builder);
            builder.RegisterModule<BusinessManagementModule>();
            builder.RegisterModule(new BusinessManagementApplicationModule());
            
            _eventPublisherMock = new Mock<IEventPublisher>();
            _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
            builder.RegisterInstance(_eventPublisherMock.Object).AsImplementedInterfaces();

            var _emailMock = new Mock<IEmailSender>();
            _emailMock.Setup(x => x.SendAsync(It.IsAny<Email>(), System.Threading.CancellationToken.None)).Verifiable();
            builder.RegisterInstance(_emailMock.Object).AsImplementedInterfaces();

            _eventFactory = new CloudEventFactory();
            builder.RegisterInstance(_eventFactory).As<IEventFactory>();
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
        public async Task ShouldCreateVerification()
        {
            // Arrange
            var context = GetContext<BusinessContext>();
            var company = await InitiateCompany(context);
            var shareholdersList = new List<ShareholderDTO>();
            shareholdersList.Add(new ShareholderDTO() { FullName = "TestName", Share = 26, Email = "Test@test.test" } );
            var command = new CreateVerificationCommand
            {
                CompanyAddressUrl = "TestUrl",
                CompanyRegistrationUrl = "TestUrl2",
                Shareholders = shareholdersList,
                CompanyId = company.Id
            };
            // Act
            var id = await SendAsync(command);
            
            // Assert
            id.Should().NotBe(Guid.Empty);
            
            var verification = await context.Verifications.FirstOrDefaultAsync(u => u.Id == id);

            verification.Should().NotBeNull();
            verification.CompanyAddressUrl.Should().Be("TestUrl");
            verification.CompanyRegistrationUrl.Should().Be("TestUrl2");
            verification.CompanyId.Should().Be(company.Id);
            verification.Shareholders.Should().NotBeEmpty();
        
        }
        
       }
}