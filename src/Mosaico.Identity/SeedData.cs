using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Persistence.SqlServer.Contexts.PersistedGrant;

namespace Mosaico.Identity
{
    public class SeedData
    {

        private static void EnsureUserExists(UserManager<ApplicationUser> userMgr, string userName, string password, string userId = null, bool isAdmin = false)
        {
            var userEntity = userMgr.FindByEmailAsync(userName).Result;
            if (userEntity == null)
            {
                userEntity = new ApplicationUser
                {
                    UserName = userName,
                    Email = userName,
                    IsAdmin = isAdmin
                };

                if (userId != null) userEntity.Id = userId;
                
                var result = userMgr.CreateAsync(userEntity, password).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Email, userName)
                };
                
                if (userEntity.IsAdmin)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, Constants.ScopesConstants.GlobalAdmin));
                }
                
                result = userMgr.AddClaimsAsync(userEntity, claims).Result;
                
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                
                var code = userMgr.GenerateEmailConfirmationTokenAsync(userEntity).Result;
                userMgr.SetLockoutEnabledAsync(userEntity, false).Wait();
                userMgr.ConfirmEmailAsync(userEntity, code).Wait();

                Console.WriteLine($@"{userName} created");
            }
            else
            {
                Console.WriteLine($@"{userName} exists");
            };
        }

        public static void EnsureSeedData(IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            if (env.IsDevelopment())
            {
                var context = scope.ServiceProvider.GetService<IIdentityContext>();
                context.RunMigration();
            }

            var contextPersistedGrant = scope.ServiceProvider.GetService<IdentityPersistedGrantDbContext>();
            contextPersistedGrant.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            EnsureUserExists(userMgr, "dev@mosaico.ai", env.IsDevelopment() ? "Mosaico1" : "swpmFvcP7jry7tRaGXHVuQLZ", "C86D8234-AAA3-4B7D-95ED-E31BC5B4E3B3", true);
        }
    }
}
