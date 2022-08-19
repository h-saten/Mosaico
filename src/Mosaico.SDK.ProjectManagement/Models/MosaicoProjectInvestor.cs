using System;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoProjectInvestor
    {
        public string UserId { get; set; }
        public bool IsAllowed { get; set; }
        public Guid StageId { get; set; }
    }
}