using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.SDK.BusinessManagement.Models
{
    public class MosaicoCompany
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public bool IsApproved { get; set; } 
        public string CreatedById { get; set; }
        public string Network { get; set; }
        public string ContractAddress { get; set; }
        public bool IsVotingEnabled { get; set; }
        public bool OnlyOwnerProposals { get; set; }
        public string InitialVotingDelay { get; set; }
        public string InitialVotingPeriod { get; set; }
        public long Quorum { get; set; }
        public string Slug { get; set; }
    }
}
