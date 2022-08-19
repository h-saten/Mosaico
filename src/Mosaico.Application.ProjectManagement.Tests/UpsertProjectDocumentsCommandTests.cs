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
using Mosaico.Application.ProjectManagement.Commands.UpsertProjectDocuments;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.ProjectContext;
using Mosaico.Tests.Base;
using NUnit.Framework;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.ProjectManagement.Tests
{
    [TestFixture]
    public class UpsertProjectDocumentsCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;

        protected override List<Profile> Profiles => new()
        {
            new ProjectManagementMapperProfile()
        };

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            _eventFactory = new CloudEventFactory();
            builder.RegisterInstance(_eventFactory).As<IEventFactory>();
            RegisterContext<ProjectContext>(builder);
            _eventPublisherMock = new Mock<IEventPublisher>();
            _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
            builder.RegisterInstance(_eventPublisherMock.Object).As<IEventPublisher>();

            builder.RegisterType<UpsertProjectDocumentsCommandHandler>().AsImplementedInterfaces();
        }

        private async Task<Project> InitializeProject(ProjectContext context)
        {
            var project = new Project
            {
                Description = "Description",
                Title = "Test Project",
                TokenId = Guid.NewGuid()
            };
            context.Add(project);
            await context.SaveChangesAsync();
            return project;
        }

        private async Task<Document> InitializeDocument(ProjectContext context)
        {
            var type = new DocumentType(Mosaico.Domain.ProjectManagement.Constants.DocumentTypes.Whitepaper, "Whitepaper");

            var document = new Document
            {
                Language = Mosaico.Base.Constants.Languages.English,
                Content = "Test content",
                Type = type
            };
            context.Add(document);
            await context.SaveChangesAsync();
            return document;
        }

        [Test]
        public async Task ShouldInsertDocument()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            var type = new DocumentType(Mosaico.Domain.ProjectManagement.Constants.DocumentTypes.Whitepaper, "Whitepaper");
            var command = new UpsertProjectDocumentsCommand
            {
                ProjectId = project.Id,
                Language = Mosaico.Base.Constants.Languages.English,
                Content = "Test content",
                Type = type?.Key
            };

            // Act
            var response = await SendAsync(command);

            // Assert
            response.Should().NotBe(Guid.Empty);

            var document = await context.Documents.FirstOrDefaultAsync(u => u.Id == response);

            document.Should().NotBeNull();
            document.Content.Should().Be("Test content");
            document.Type.Should().Be(type);
            document.Language.Should().Be(Mosaico.Base.Constants.Languages.English);
        }

        [Test]
        public async Task ShouldUpdateExistingDocument()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            var document = await InitializeDocument(context);
            var newType = new DocumentType(Mosaico.Domain.ProjectManagement.Constants.DocumentTypes.PrivacyPolicy, "Privacy Policy");

            var command2 = new UpsertProjectDocumentsCommand
            {
                ProjectId = project.Id,
                Language = Mosaico.Base.Constants.Languages.Polish,
                Content = "Test content2",
                Type = newType?.Key
            };
            // Act
            var response2 = await SendAsync(command2);
            //Assert
            var newDocument = await context.Documents.FirstOrDefaultAsync(u => u.Id == response2);

            newDocument.Should().NotBeNull();
            newDocument.Content.Should().Be("Test content2");
            newDocument.Type.Should().Be(newType);
            newDocument.Language.Should().Be(Mosaico.Base.Constants.Languages.Polish);
        }
    }
}