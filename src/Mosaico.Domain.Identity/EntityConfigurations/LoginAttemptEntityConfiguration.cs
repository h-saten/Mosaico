using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class LoginAttemptEntityConfiguration : EntityConfigurationBase<LoginAttempt>
    {
        protected override string TableName => Constants.Tables.LoginAttempts;
        protected override string Schema => Constants.Schema;
    }
}