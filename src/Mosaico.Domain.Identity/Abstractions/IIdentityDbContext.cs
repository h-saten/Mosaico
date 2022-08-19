using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.Abstractions
{
    public interface IIdentityContext : IDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<UserToPermission> UserToPermission { get; set; }
        DbSet<SecurityCode> SecurityCodes { get; set; }
        DbSet<DeletionRequest> DeletionRequests { get; set; }
        DbSet<PhoneNumberConfirmationCode> PhoneNumberConfirmationCodes { get; set; }
        DbSet<LoginAttempt> LoginAttempts { get; set; }
        DbSet<AuthorizedDevice> AuthorizedDevices { get; set; }
        DbSet<KangaUser> KangaUsers { get; set; }
        DbSet<NewsletterSubscribers> NewsletterSubscribers { get; set; }
        DbSet<UserEvaluationQuestion> UserEvaluationQuestions { get; set; }
        DbSet<KycVerification> KycVerifications { get; set; }
    }
}