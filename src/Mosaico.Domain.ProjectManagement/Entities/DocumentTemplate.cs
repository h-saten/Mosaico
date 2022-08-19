using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class DocumentTemplate : EntityBase
    {
        public string Content { get; set; }
        public string Key { get; set; }
        public string TemplateVersion { get; set; }
        public string Language { get; set; }
        public bool IsEnabled { get; set; }
    }
}