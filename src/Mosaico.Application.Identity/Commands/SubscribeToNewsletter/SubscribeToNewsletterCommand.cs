using System;
using System.Text.Json.Serialization;
using MediatR;


namespace Mosaico.Application.Identity.Commands.SubscribeToNewsletter
{
    public class SubscribeToNewsletterCommand : IRequest
    {
        public string Email { get; set; }
    }
}
