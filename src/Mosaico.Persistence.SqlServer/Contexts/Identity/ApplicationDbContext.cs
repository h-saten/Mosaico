using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mosaico.Authorization.Base;
using Mosaico.Core.EntityFramework.Encryption;
using Mosaico.Core.EntityFramework.Extensions;
using Mosaico.Domain.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Extensions;

namespace Mosaico.Persistence.SqlServer.Contexts.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IIdentityContext
     {
         private const string pswd = "4@7V3UB}=F!,(svaH4J3ak3?YQ:A\"!^x.VBX@B|sz.X#iZ!c5_~3;-zr_'M6@ebZT.yX-\"(.8eV6LsnjM-&#xJaNy/gNzXR@Y])hkcMUY4t8[Q7gG{JX)|fk";

         private readonly ICurrentUserContext _currentUserContext;

         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserContext currentUserContext = null) : base(options)
         {
             _currentUserContext = currentUserContext;
         }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyEncryptionValueConverter(pswd);
            builder.ApplyIdentityConfiguration();
        }

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            return Database.BeginTransaction(isolationLevel);
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.MarkCreated(_currentUserContext?.UserId, entry.Entity.CreatedAt);
                        break;
                    case EntityState.Modified:
                        entry.Entity.MarkModified(_currentUserContext?.UserId, entry.Entity.CreatedAt);
                        break;
                    
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public void RunMigration()
        {
            Database.Migrate();
        }

        public string Encrypt(string data)
        {
            return data.Encrypt(pswd);
        }

        public string Decrypt(string encryptedData)
        {
            return encryptedData.Decrypt(pswd);
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserToPermission> UserToPermission { get; set; }
        public DbSet<SecurityCode> SecurityCodes { get; set; }
        public DbSet<DeletionRequest> DeletionRequests { get; set; }
        public DbSet<PhoneNumberConfirmationCode> PhoneNumberConfirmationCodes { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<AuthorizedDevice> AuthorizedDevices { get; set; }
        public DbSet<KangaUser> KangaUsers { get; set; }
        public DbSet<NewsletterSubscribers> NewsletterSubscribers { get; set; }
        public DbSet<UserEvaluationQuestion> UserEvaluationQuestions { get; set; }
        public DbSet<KycVerification> KycVerifications { get; set; }

        public string ContextName => "identity";
     }
}