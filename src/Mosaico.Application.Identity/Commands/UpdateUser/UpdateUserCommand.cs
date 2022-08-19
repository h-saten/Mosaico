using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.UpdateUser
{
    [Restricted(nameof(Id), Authorization.Base.Constants.DefaultRoles.Self)]
    public class UpdateUserCommand : IRequest
    {
        [JsonIgnore]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Timezone { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public DateTime? Dob { get; set; }
    }
}