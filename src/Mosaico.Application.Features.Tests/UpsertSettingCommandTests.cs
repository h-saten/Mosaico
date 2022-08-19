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
using Mosaico.Application.Features.Commands.UpsertSetting;
using Mosaico.Domain.Features;
using Mosaico.Domain.Features.Entities;
using Mosaico.Domain.Features.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.FeaturesContext;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.Features.Tests
{
    public class UpsertSettingCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private Mock<IUserManagementClient> _managementClientMock;

        protected override List<Profile> Profiles => new List<Profile>
        {
            new FeaturesMapperProfile()
        };

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            
            RegisterContext<FeaturesContext>(builder);
            builder.RegisterModule<FeaturesModule>();
            builder.RegisterModule(new FeaturesApplicationModule());
            
            _eventPublisherMock = new Mock<IEventPublisher>();
            _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
            builder.RegisterInstance(_eventPublisherMock.Object).AsImplementedInterfaces();

            _managementClientMock = new Mock<IUserManagementClient>();
            builder.RegisterInstance(_managementClientMock.Object).As<IUserManagementClient>();

        }

        private async Task<Feature> InitiateFeature(FeaturesContext context)
        {
            var feature = new Feature
            {
                EntityId = Guid.NewGuid(),
                Value = "Test Value",
                FeatureName = "Test Name"
            };
            context.Add(feature);
            await context.SaveChangesAsync();
            return feature;
        }

        [Test]
        public async Task ShouldCreateFeature()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var command = new UpsertSettingCommand
            {
                Value = "Test Value",
                EntityId = entityId,
                FeatureName = "Test Name"
            };
            // Act
            var id = await SendAsync(command);
            
            // Assert
            
            var context = GetContext<FeaturesContext>();
            var feature = await context.Features.FirstOrDefaultAsync(u => u.EntityId == entityId);

            feature.Should().NotBeNull();
            feature.EntityId.Should().Be(entityId);
            feature.Value.Should().Be("Test Value");
            feature.FeatureName.Should().Be("Test Name");
            feature.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
            feature.ModifiedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
        
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);
        }

        [Test]
        public async Task ShouldUpdateFeature()
        {
            // Arrange
            var context = GetContext<FeaturesContext>();
            var initialFeature = await InitiateFeature(context);
            var command = new UpsertSettingCommand
            {
                Value = "Test Value2",
                EntityId = initialFeature.EntityId,
                FeatureName = "Test Name2"
            };
            // Act
            var id = await SendAsync(command);

            // Assert

            var feature = await context.Features.FirstOrDefaultAsync(u => u.EntityId == initialFeature.EntityId);

            feature.Should().NotBeNull();
            feature.EntityId.Should().Be(initialFeature.EntityId);
            feature.Value.Should().Be("Test Value2");
            feature.FeatureName.Should().Be("Test Name2");
            feature.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
            feature.ModifiedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);
        }
    }
}