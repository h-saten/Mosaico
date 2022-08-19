using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class ProjectRole : EntityBase
    {
        public string Title { get; set; }
        public string Key { get; set; }
    }
}