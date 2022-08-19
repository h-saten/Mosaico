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
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.ProjectContext;
using Mosaico.Tests.Base;
using NUnit.Framework;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.ProjectManagement.Tests
{
    // [TestFixture]
    // public class UpsertProjectVestingCommandTests : EFInMemoryTestBase
    // {
    //     private Mock<IEventPublisher> _eventPublisherMock;
    //     private IEventFactory _eventFactory;
    //
    //     protected override List<Profile> Profiles => new()
    //     {
    //         new ProjectManagementMapperProfile()
    //     };
    //     
    //     protected override void RegisterDependencies(ContainerBuilder builder)
    //     {
    //         base.RegisterDependencies(builder);
    //         _eventFactory = new CloudEventFactory();
    //         builder.RegisterInstance(_eventFactory).As<IEventFactory>();
    //         RegisterContext<ProjectContext>(builder);
    //         _eventPublisherMock = new Mock<IEventPublisher>();
    //         _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
    //         builder.RegisterInstance(_eventPublisherMock.Object).As<IEventPublisher>();
    //
    //         builder.RegisterType<UpsertProjectVestingCommandHandler>().AsImplementedInterfaces();
    //         builder.RegisterType<UpsertProjectVestingCommandValidator>().As<IValidator<UpsertProjectVestingCommand>>();
    //     }
    //     
    //     private async Task<Project> InitializeProject(ProjectContext context)
    //     {
    //         var project = new Project
    //         {
    //             Description = "Description",
    //             Network = Mosaico.Blockchain.Base.Constants.BlockchainNetworks.Ethereum,
    //             Title = "Test Project",
    //             TokenId = Guid.NewGuid(),
    //             IsVestingEnabled = false,
    //             IsStakingEnabled = false,
    //             Stages = new List<Stage>
    //             {
    //                 new Stage
    //                 {
    //                     Name = "Test Stage",
    //                     StartDate = DateTimeOffset.UtcNow.AddDays(6),
    //                     EndDate = DateTimeOffset.UtcNow.AddDays(6).AddDays(3),
    //                     MinimumPurchase = 100,
    //                     MaximumPurchase = 1000,
    //                     TokenPrice = (decimal) 0.25,
    //                     TokensSupply = 10000
    //                 }
    //             }
    //         };
    //         project.SetStatus(context.ProjectStatuses.FirstOrDefault());
    //         context.Add(project);
    //         await context.SaveChangesAsync();
    //         return project;
    //     }
    //     
    //     [Test]
    //     public async Task ShouldInsertVesting()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 365,
    //                     Distribution = 1000,
    //                     Name = "Fund 1",
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                             Email = "d@mosaico.ai"
    //                         }
    //                     }
    //                 },
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 60,
    //                     Distribution = 1000,
    //                     Name = "Fund 2",
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(60),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                             Email = "m@mosaico.ai"
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         
    //         // Act
    //         var response = await SendAsync(command);
    //         
    //         // Assert
    //         response.Should().NotBe(Guid.Empty);
    //         
    //         var dbProject = await context.Projects.FirstOrDefaultAsync(u => u.VestingId == response);
    //        
    //         dbProject.Should().NotBeNull();
    //         dbProject.Vesting.Should().NotBeNull();
    //         dbProject.Vesting.Funds.Count.Should().Be(2);
    //     }
    //
    //     [Test]
    //     public async Task ShouldUpdateExistingVesting()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         project.Vesting = new Vesting
    //         {
    //             Funds = new List<VestingFund>
    //             {
    //                 new VestingFund
    //                 {
    //                     Days = 60,
    //                     Distribution = 1000,
    //                     Name = "Fund 2",
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(60),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25,
    //                     Invitations = new List<VestingFundInvitation>
    //                     {
    //                         new VestingFundInvitation
    //                         {
    //                             Email = "m@mosaico.ai"
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         context.Update(project);
    //         await context.SaveChangesAsync();
    //         var fundId = project.Vesting.Funds.FirstOrDefault();
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Id = fundId.Id,
    //                     Days = 365,
    //                     Distribution = 1000,
    //                     Name = "Fund 1",
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                             Phone = "66666666"
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         // Act
    //         var response = await SendAsync(command);
    //         //Assert
    //         var dbProject = await context.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);
    //         dbProject.Should().NotBeNull();
    //         dbProject.Vesting.Should().NotBeNull();
    //         dbProject.Vesting.Funds.Count.Should().Be(1);
    //         var fund = dbProject.Vesting.Funds.FirstOrDefault();
    //         fund.Days.Should().Be(365);
    //         fund.Name.Should().Be("Fund 1");
    //         fund.Invitations.Count.Should().Be(1);
    //         var invitation = fund.Invitations.FirstOrDefault();
    //         invitation.Phone.Should().Be("66666666");
    //         invitation.Email.Should().BeNullOrEmpty();
    //     }
    //     
    //     [Test]
    //     public async Task ShouldInsertVestingFundWithStage()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         var stage = await context.Stages.FirstOrDefaultAsync(s => s.ProjectId == project.Id);
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 365,
    //                     Distribution = 1000,
    //                     Name = "Fund 1",
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25,
    //                     StageId = stage.Id,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                             Email = "d@mosaico.ai"
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         
    //         // Act
    //         var response = await SendAsync(command);
    //         
    //         // Assert
    //         response.Should().NotBe(Guid.Empty);
    //         
    //         var dbProject = await context.Projects.FirstOrDefaultAsync(u => u.VestingId == response);
    //        
    //         dbProject.Should().NotBeNull();
    //         dbProject.Vesting.Should().NotBeNull();
    //         dbProject.Vesting.Funds.Count.Should().Be(1);
    //         dbProject.Vesting.Funds.First().Stage.Should().NotBeNull();
    //         dbProject.Vesting.Funds.First().StageId.Should().Be(stage.Id);
    //     }
    //     
    //     [Test]
    //     public async Task ShouldFailIfVestingHasNoInvitations()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 365,
    //                     Distribution = 1000,
    //                     Name = "Fund 1",
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25
    //                 }
    //             }
    //         };
    //         
    //         // Act
    //         // Assert
    //         Func<Task> action = async () =>
    //         {
    //             await SendAsync(command);
    //         };
    //         // Assert
    //         await action.Should().ThrowAsync<ValidationException>()
    //             .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
    //     }
    //     
    //     [Test]
    //     public async Task ShouldFailIfVestingDistributionIsTooSmall()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 365,
    //                     Distribution = 0,
    //                     Name = "Fund 1",
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                             Email = "m@mosaico.ai"
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         
    //         // Act
    //         // Assert
    //         Func<Task> action = async () =>
    //         {
    //             await SendAsync(command);
    //         };
    //         // Assert
    //         await action.Should().ThrowAsync<ValidationException>()
    //             .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
    //     }
    //     
    //     [Test]
    //     public async Task ShouldFailIfVestingDurationIsTooSmall()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 0,
    //                     Distribution = 1000,
    //                     Name = "Fund 1",
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                             Email = "m@mosaico.ai"
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         
    //         // Act
    //         // Assert
    //         Func<Task> action = async () =>
    //         {
    //             await SendAsync(command);
    //         };
    //         // Assert
    //         await action.Should().ThrowAsync<ValidationException>()
    //             .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
    //     }
    //     
    //     [Test]
    //     public async Task ShouldFailIfVestingNameIsWrong()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 365,
    //                     Distribution = 1000,
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                             Email = "m@mosaico.ai"
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         
    //         // Act
    //         // Assert
    //         Func<Task> action = async () =>
    //         {
    //             await SendAsync(command);
    //         };
    //         // Assert
    //         await action.Should().ThrowAsync<ValidationException>()
    //             .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
    //     }
    //     
    //     [Test]
    //     public async Task ShouldFailIfInvitationHasNoContactDetails()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 365,
    //                     Distribution = 1000,
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 25,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         
    //         // Act
    //         // Assert
    //         Func<Task> action = async () =>
    //         {
    //             await SendAsync(command);
    //         };
    //         // Assert
    //         await action.Should().ThrowAsync<ValidationException>()
    //             .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
    //     }
    //     
    //     [Test]
    //     public async Task ShouldFailIfSubtractAmountIsTooSmall()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 365,
    //                     Distribution = 1000,
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = -1,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                             Email = "m@mosaico.ai"
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         
    //         // Act
    //         // Assert
    //         Func<Task> action = async () =>
    //         {
    //             await SendAsync(command);
    //         };
    //         // Assert
    //         await action.Should().ThrowAsync<ValidationException>()
    //             .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
    //     }
    //     
    //     [Test]
    //     public async Task ShouldFailIfSubtractAmountIsTooBig()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var project = await InitializeProject(context);
    //         var command = new UpsertProjectVestingCommand
    //         {
    //             ProjectId = project.Id,
    //             Funds = new List<CreateVestingFundDTO>
    //             {
    //                 new CreateVestingFundDTO
    //                 {
    //                     Days = 365,
    //                     Distribution = 1000,
    //                     StartAt = DateTimeOffset.UtcNow,
    //                     EndAt = DateTimeOffset.UtcNow.AddDays(365),
    //                     CanWithdrawEarly = false,
    //                     SubtractedPercent = 110,
    //                     Invitations = new List<CreateVestingInvitationDTO>
    //                     {
    //                         new CreateVestingInvitationDTO
    //                         {
    //                             Email = "m@mosaico.ai"
    //                         }
    //                     }
    //                 }
    //             }
    //         };
    //         
    //         // Act
    //         // Assert
    //         Func<Task> action = async () =>
    //         {
    //             await SendAsync(command);
    //         };
    //         // Assert
    //         await action.Should().ThrowAsync<ValidationException>()
    //             .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
    //     }
    // }
}