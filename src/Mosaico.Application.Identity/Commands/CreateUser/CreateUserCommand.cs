using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Mosaico.Application.Identity.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool? Terms { get; set; }
        public bool? NotForbiddenCitizenship { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [JsonIgnore]
        public bool IsConfirmed { get; set; }

        public bool NewsletterPersonalDataProcessing { get; set; } = false;
        
        public string ReturnUrl { get; set; }
        public string Language { get; set; }
    }
}