using Mosaico.Domain.BusinessManagement;
using System.Collections.Generic;

namespace Mosaico.Application.BusinessManagement.Permissions
{
    public class CompanyPermissions : Dictionary<string, bool>
    {
        public CompanyPermissions()
        {
            foreach (var key in Authorization.Base.Constants.Permissions.Company.GetAll())
            {
                Add(key, false);
            }
        }
    }
}