using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Features.Commands.UpdateBetaTester
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class UpdateBetaTesterCommand : IRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
    }
}