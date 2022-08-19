using System;
using System.Collections.Generic;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class ProjectDetailDTO : ProjectDTO
    {
        public Guid? CompanyId { get; set; }
        public Guid? LegacyId { get; set; }
        public List<string> PaymentMethods { get; set; }
        public List<string> PaymentCurrencies { get; set; }
        public List<StageDTO> Stages { get; set; }
    }
}