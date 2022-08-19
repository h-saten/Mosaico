using System;
using System.Collections.Generic;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoVesting
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public List<MosaicoVestingFund> Funds { get; set; }
    }
}