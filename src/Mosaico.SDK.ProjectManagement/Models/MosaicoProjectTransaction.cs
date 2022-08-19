using System;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoProjectTransaction
    {
        public Guid Id { get; set; }
        public Guid? TokenId { get; set; }
        public string ProjectName { get; set; }
    }
}