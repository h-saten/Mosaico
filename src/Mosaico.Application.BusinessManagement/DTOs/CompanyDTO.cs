using System;

namespace Mosaico.Application.BusinessManagement.DTOs
{
    public class CompanyDTO
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string VATId { get; set; }
        public string Size { get; set; }
        public bool IsApproved { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LogoUrl { get; set; }
        public string Region { get; set; }
        public string Network { get; set; }
        public bool IsVotingEnabled { get; set; }
        public bool IsEverybodyCanVoteEnabled { get; set; }
        public bool IsSubscribed { get; set; }
        public string Slug { get; set; }
    }
}