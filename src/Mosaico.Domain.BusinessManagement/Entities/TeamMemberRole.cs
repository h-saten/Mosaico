using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.BusinessManagement.Entities
{
    public class TeamMemberRole : EntityBase
    {
        public string Title { get; set; }
        public string Key { get; set; }

    }
}