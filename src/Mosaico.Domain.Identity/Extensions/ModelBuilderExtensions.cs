using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.EntityConfigurations;

namespace Mosaico.Domain.Identity.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyIdentityConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserEntityConfiguration());
            builder.ApplyConfiguration(new PermissionEntityConfiguration());
            builder.ApplyConfiguration(new UserToPermissionEntityConfiguration());
            builder.ApplyConfiguration(new DeletionRequestEntityConfiguration());
            builder.ApplyConfiguration(new PhoneNumberConfirmationCodeEntityConfiguration());
            builder.ApplyConfiguration(new LoginAttemptEntityConfiguration());
            builder.ApplyConfiguration(new KangaUserConfiguration());
            builder.ApplyConfiguration(new NewsletterSubscribersConfiguration());
            builder.ApplyConfiguration(new UserEvaluationQuestionEntityConfiguration());
            builder.ApplyConfiguration(new KycVerificationStatusEntityConfiguration());
        }
    }
}