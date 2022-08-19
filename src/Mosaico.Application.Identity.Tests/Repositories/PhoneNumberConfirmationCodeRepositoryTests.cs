using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Repositories;
using Mosaico.Domain.Identity.ValueObjects;
using Mosaico.Persistence.SqlServer.Contexts.Identity;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.Identity.Tests.Repositories
{
    public class PhoneNumberConfirmationCodeRepositoryTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new();

        private ICurrentUserContext _userContext;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<ApplicationDbContext>(builder);
            builder.RegisterModule(new IdentityApplicationModule());

            _userContext = CreateCurrentUserContextMock();
            builder.RegisterInstance(_userContext).AsImplementedInterfaces();
        }

        private ApplicationUser AddUser()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var phoneNumber = "+48886163743";
            var userId = _userContext.UserId;
            var user = Builder<ApplicationUser>.CreateNew().Build();
            user.Id = userId;
            user.PhoneNumber = phoneNumber;
            dbContext.Users.Add(user);
            return user;
        }
        
        [Test]
        public async Task ShouldReturnNullWhenCodeNotExist()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var sut = new PhoneNumberConfirmationCodesRepository(dbContext);
            Assert.Null(await sut.GetLastlyGeneratedCodeAsync(Guid.NewGuid().ToString()));
        }

        [Test]
        public async Task ShouldCreateCode()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var user = AddUser();

            var sut = new PhoneNumberConfirmationCodesRepository(dbContext);
            await dbContext.SaveChangesAsync();

            var generatedCode = await sut.CreateCodeAsync(user.Id, new PhoneNumber(user.PhoneNumber));
            
            var results = await dbContext
                .PhoneNumberConfirmationCodes
                .Where(x => x.UserId == user.Id)
                .CountAsync();

            var lastGeneratedCode = await sut.GetLastlyGeneratedCodeAsync(user.Id);
            Assert.NotNull(lastGeneratedCode);
            Assert.AreEqual(lastGeneratedCode.Code, generatedCode.Code);
            Assert.AreEqual(1, results);
        }

        [Test]
        public async Task ShouldMarkCodeAsUsed()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var user = AddUser();

            var sut = new PhoneNumberConfirmationCodesRepository(dbContext);
            await dbContext.SaveChangesAsync();

            var code = await sut.CreateCodeAsync(user.Id, new PhoneNumber(user.PhoneNumber));
            await sut.SetSecurityCodeUsed(code.Id);
            
            var results = await dbContext
                .PhoneNumberConfirmationCodes
                .Where(x => x.UserId == user.Id && x.Used)
                .CountAsync();
            
            Assert.Null(await sut.GetLastlyGeneratedCodeAsync(user.Id));
            Assert.AreEqual(1, results);
        }
    }
}