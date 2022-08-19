using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class UserEvaluationQuestionEntityConfiguration : EntityConfigurationBase<UserEvaluationQuestion>
    {
        protected override string TableName => Constants.Tables.UserEvaluationQuestions;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<UserEvaluationQuestion> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => new {t.Key, t.UserId});
        }
    }
}