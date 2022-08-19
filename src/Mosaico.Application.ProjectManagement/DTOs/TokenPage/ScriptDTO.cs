using System;

namespace Mosaico.Application.ProjectManagement.DTOs.TokenPage
{
    public class ScriptDTO
    {
        public Guid Id { get; set; }
        public string Src { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public bool IsExternal { get; set; }
        public int Order { get; set; }
    }
}