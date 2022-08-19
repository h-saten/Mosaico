using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.EntityConfigurations;

namespace Mosaico.Domain.BusinessManagement.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyBusinessManagementConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CompanyEntityConfiguration());
            builder.ApplyConfiguration(new TeamMemberEntityConfiguration());
            builder.ApplyConfiguration(new TeamMemberRoleEntityConfiguration());
            builder.ApplyConfiguration(new VerificationEntityConfiguration());
            builder.ApplyConfiguration(new ShareholderEntityConfiguration());
            builder.ApplyConfiguration(new CompanySubscriberEntityConfiguration());
            builder.ApplyConfiguration(new ProposalEntityConfiguration());
            builder.ApplyConfiguration(new VoteEntityConfiguration());
            builder.ApplyConfiguration(new DocumentEntityConfiguration());
        }
    }
}