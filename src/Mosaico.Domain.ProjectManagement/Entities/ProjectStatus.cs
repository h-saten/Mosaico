using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class ProjectStatus : EntityBase
    {
        public ProjectStatus()
        {
            Id = Guid.NewGuid();
        }

        public ProjectStatus(string key, string title)
        {
            Id = Guid.NewGuid();
            Key = key;
            Title = title;
        }
        
        public string Key { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}