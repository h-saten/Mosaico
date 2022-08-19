using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Features.Commands.UpsertSetting
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class UpsertSettingCommand : IRequest
    {
        public Guid Id { get; set; }
        public string FeatureName { get; set; }
        public string Value { get; set; }
        public Guid? EntityId { get; set; }
    }
}