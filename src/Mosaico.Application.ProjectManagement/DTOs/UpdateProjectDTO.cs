using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class UpdateProjectDTO
    {
        public string ShortDescription { get; set; }
        public string Title { get; set; }
        public Guid? TokenId { get; set; }
    }
}