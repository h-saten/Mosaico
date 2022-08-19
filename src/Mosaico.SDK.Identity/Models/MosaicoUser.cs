using System;

namespace Mosaico.SDK.Identity.Models
{
    public class MosaicoUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAMLVerified { get; set; }
        public bool EvaluationCompleted { get; set; }
        public string PhoneNumber { get; set; }
        public string Language { get; set; }
    }
}