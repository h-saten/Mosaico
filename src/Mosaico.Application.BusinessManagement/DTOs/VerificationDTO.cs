using System;
using System.Collections.Generic;

namespace Mosaico.Application.BusinessManagement.DTOs
{
    public class VerificationDTO
    {
        public Guid Id { get; set; }
        public string CompanyRegistrationUrl { get; set; }
        public string CompanyAddressUrl { get; set; }
        public List<ShareholderDTO> Shareholders { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}