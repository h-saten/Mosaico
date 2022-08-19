using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UpsertPageReview
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpsertPageReviewCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid? Id { get; set; }
        
        public string Link { get; set; }
        
        public bool IsHidden { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public PageReviewCategory Category { get; set; }
        [JsonIgnore]
        public Guid PageId { get; set; }
    }
}