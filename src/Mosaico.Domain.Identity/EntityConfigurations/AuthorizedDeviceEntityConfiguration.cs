using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.EntityConfigurations
{
    public class AuthorizedDeviceEntityConfiguration : EntityConfigurationBase<AuthorizedDevice>
    {
        protected override string TableName => Constants.Tables.AuthorizedDevices;
        protected override string Schema => Constants.Schema;
    }
}