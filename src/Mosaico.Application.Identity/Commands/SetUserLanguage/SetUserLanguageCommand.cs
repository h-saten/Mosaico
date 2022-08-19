using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.SetUserLanguage
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class SetUserLanguageCommand : IRequest
    {
        public string Language { get; set; }
        
        [JsonIgnore]
        public string UserId { get; set; }
    }
}