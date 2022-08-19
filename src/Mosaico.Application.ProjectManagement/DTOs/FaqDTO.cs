using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class FaqDTO
    {
        public Guid Id { get; set; }
        public bool IsHidden { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
    }
}