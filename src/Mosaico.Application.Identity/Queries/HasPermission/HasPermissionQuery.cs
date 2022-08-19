using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.HasPermission
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public record HasPermissionQuery : IRequest<bool>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string Key { get; init; }
        public Guid? EntityId { get; init; }
    }
}