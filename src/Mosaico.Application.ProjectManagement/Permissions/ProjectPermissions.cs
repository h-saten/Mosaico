using System.Collections.Generic;

namespace Mosaico.Application.ProjectManagement.Permissions
{
    public class ProjectPermissions : Dictionary<string, bool>
    {
        public ProjectPermissions()
        {
            foreach (var key in Authorization.Base.Constants.Permissions.Project.GetAll())
            {
                Add(key, false);
            }
        }
    }
}