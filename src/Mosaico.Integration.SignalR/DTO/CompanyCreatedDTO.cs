using System;

namespace Mosaico.Integration.SignalR.DTO
{
    public class CompanyCreatedDTO
    {
        public Guid CompanyId { get; set; }
        public string Slug { get; set; }
    }
}