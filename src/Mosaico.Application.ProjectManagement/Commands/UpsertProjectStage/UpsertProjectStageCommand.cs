using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertProjectStage
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpsertProjectStageCommand : IRequest<Guid>
    {
        public Guid ProjectId { get; set; }
        public Guid? StageId { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StageType Type { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public decimal TokenSupply { get; set; }
        public decimal TokenPrice { get; set; }
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
    }
}