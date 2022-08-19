using System;
using System.Collections.Generic;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoProject
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string CreatedById { get; set; }
        public string LogoUrl { get; set; }
        public List<ProjectStage> Stages { get; set; } = new List<ProjectStage>();
        public Guid? TokenId { get; set; }
        public Guid? CompanyId { get; set; }
    }
}