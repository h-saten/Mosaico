using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Features.Entities;

namespace Mosaico.Domain.Features.EntityConfigurations
{
    public class TestSubmissionEntityConfiguration : EntityConfigurationBase<TestSubmission>
    {
        protected override string TableName => Constants.Tables.TestSubmissions;
        protected override string Schema => Constants.Schema;
    }
}