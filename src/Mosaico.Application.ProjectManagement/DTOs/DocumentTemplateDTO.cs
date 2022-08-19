using Mosaico.Domain.ProjectManagement.Entities;
using System;
using System.Collections.Generic;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class DocumentTemplateDTO
    {
        public string Content { get; set; }
        public string Key { get; set; }
        public string TemplateVersion { get; set; }
        public string Language { get; set; }
        public bool IsEnabled { get; set; }
        public string Id { get; set; }
    }
}