using System;

namespace Mosaico.Application.Identity.Queries.GetUser
{
    public class GetUserQueryResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAMLVerified { get; set; }
        public string PhotoUrl { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsDeactivated { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime LastLogin { get; set; }
        public string Timezone { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public DateTime? Dob { get; set; }
        public bool HasKangaAccount { get; set; }
        public string Language { get; set; }
        public bool EvaluationCompleted { get; set; }
    }
}