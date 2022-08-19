using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    //TODO: To translation
    public class StageStatus : EntityBase
    {
        public StageStatus()
        {
            Id = Guid.NewGuid();
        }

        public StageStatus(string key, string title)
        {
            Id = Guid.NewGuid();
            Key = key;
            Title = title;
        }
        
        public string Key { get; set; }
        public string Title { get; set; }
    }
}