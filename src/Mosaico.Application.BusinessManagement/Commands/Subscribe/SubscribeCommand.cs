using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.BusinessManagement.Queries.GetCompany;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.BusinessManagement.Commands.Subscribe
{
    [CacheReset(nameof(GetCompanyQuery), "{{CompanyId}}")]
    public class SubscribeCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string Email { get; set; }
        
        [JsonIgnore]
        public Guid CompanyId { get; set; }
    }
}