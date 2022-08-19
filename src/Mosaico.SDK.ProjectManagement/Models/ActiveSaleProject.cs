using System;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class ActiveSaleProject
    {
        public MosaicoProject Project { get; set; }
        public Guid ActiveStageId { get; set; }
    }
}