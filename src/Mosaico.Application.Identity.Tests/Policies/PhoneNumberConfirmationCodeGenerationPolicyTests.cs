using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Moq;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Policies;
using Mosaico.Domain.Identity.Repositories;
using Mosaico.Domain.Identity.ValueObjects;
using Mosaico.Persistence.SqlServer.Contexts.Identity;
using Mosaico.Tests.Base;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Identity.Tests.Policies
{
    public class PhoneNumberConfirmationCodeGenerationPolicyTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new();

        private Mock<ILogger> _logger;
        private ICurrentUserContext _userContext;
        private IPhoneNumberConfirmationCodesRepository _phoneNumberConfirmationCodesRepository;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            var context = RegisterContext<ApplicationDbContext>(builder);
            builder.RegisterModule(new IdentityApplicationModule());
            
            _logger = new Mock<ILogger>();
            builder.RegisterInstance(_logger.Object).AsImplementedInterfaces();
            
            var userContext = CreateCurrentUserContextMock();
            _userContext = userContext;

            _phoneNumberConfirmationCodesRepository = new PhoneNumberConfirmationCodesRepository(context);
        }

        [Test]
        public async Task ShouldNotAllowGenerateCodeForExistingPhoneNumber()
        {
            var userId = _userContext.UserId.ToString();
            var context = GetContext<ApplicationDbContext>();
            var phoneNumber = new PhoneNumber("+48886163742");

            var user = new ApplicationUser
            {
                Email = "test1@mosaico.ai",
                PhoneNumber = phoneNumber.ToString()
            };
            context.Users.Add(user);
            
            await context.SaveChangesAsync();
            var sut = new PhoneNumberConfirmationCodeGenerationPolicy(context, _phoneNumberConfirmationCodesRepository);
            
            var result = await sut.CanGenerate(userId, phoneNumber);
            
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ShouldNotAllowGenerateCodeIfWasAlreadyGenerateInLast2Minutes()
        {
            var userId = _userContext.UserId.ToString();
            var context = GetContext<ApplicationDbContext>();
            var phoneNumber = new PhoneNumber("+48886163742");
            
            var code1 = new PhoneNumberConfirmationCode(userId, phoneNumber, "testcode1")
            {
                CreatedAt = DateTimeOffset.UtcNow
            };
            context.PhoneNumberConfirmationCodes.Add(code1);
            await context.SaveChangesAsync();
            
            var sut = new PhoneNumberConfirmationCodeGenerationPolicy(context, _phoneNumberConfirmationCodesRepository);
            
            var result = await sut.CanGenerate(userId, phoneNumber);
            
            Assert.IsFalse(result);
        }
        
        [Test]
        public async Task ShouldNotAllowGenerateCodeIfWas3CodeGeneratedInLastHour()
        {
            var userId = _userContext.UserId.ToString();
            var context = GetContext<ApplicationDbContext>();
            var phoneNumber = new PhoneNumber("+48886163742");
            
            var code1 = new PhoneNumberConfirmationCode(userId, new PhoneNumber("+48886163742"), "testcode1")
            {
                CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-5)
            };
            var code2 = new PhoneNumberConfirmationCode(userId, new PhoneNumber("+48886163742"), "testcode2")
            {
                CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-10)
            };
            var code3 = new PhoneNumberConfirmationCode(userId, new PhoneNumber("+48886163742"), "testcode3")
            {
                CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-15)
            };
            context.PhoneNumberConfirmationCodes.AddRange(code1, code2, code3);
            await context.SaveChangesAsync();
            
            var sut = new PhoneNumberConfirmationCodeGenerationPolicy(context, _phoneNumberConfirmationCodesRepository);
            
            var result = await sut.CanGenerate(userId, phoneNumber);
            
            Assert.IsFalse(result);
        } 
        
        [Test]
        public async Task ShouldAllowGenerateCodeIfWas2CodeGeneratedInLastHour()
        {
            var userId = _userContext.UserId;
            var context = GetContext<ApplicationDbContext>();
            var phoneNumber = new PhoneNumber("+48886163742");
            
            var code1 = new PhoneNumberConfirmationCode(userId, new PhoneNumber("+48886163742"))
            {
                CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-5), 
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(-3)
            };
            var code2 = new PhoneNumberConfirmationCode(userId, new PhoneNumber("+48886163742"))
            {
                CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-10), 
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(-8)
            };

            context.PhoneNumberConfirmationCodes.AddRange(code1, code2);
            await context.SaveChangesAsync();
            var sut = new PhoneNumberConfirmationCodeGenerationPolicy(context, _phoneNumberConfirmationCodesRepository);
            
            var result = await sut.CanGenerate(userId, phoneNumber);
            
            Assert.IsTrue(result);
        } 
        
        [Test]
        public async Task ShouldAllowGenerateCodeForTheFirstTry()
        {
            var userId = _userContext.UserId.ToString();
            var context = GetContext<ApplicationDbContext>();
            var phoneNumber = new PhoneNumber("+48886163742");

            var sut = new PhoneNumberConfirmationCodeGenerationPolicy(context, _phoneNumberConfirmationCodesRepository);
            
            var result = await sut.CanGenerate(userId, phoneNumber);
            
            Assert.IsTrue(result);
        } 
    }
}