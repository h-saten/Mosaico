using System;
using MediatR;
using Mosaico.Application.BusinessManagement.Queries.GetCompany;
using Mosaico.Cache.Base.Attributes;
using Newtonsoft.Json;

namespace Mosaico.Application.BusinessManagement.Commands.Unsubscribe
{
    [CacheReset(nameof(GetCompanyQuery), "{{CompanyId}}")]
    public class UnsubscribeCommand : IRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }
        
        [JsonIgnore]
        public Guid CompanyId { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
    }
}