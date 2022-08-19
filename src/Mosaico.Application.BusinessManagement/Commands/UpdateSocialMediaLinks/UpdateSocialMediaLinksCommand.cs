using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.BusinessManagement.Queries.GetCompany;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.BusinessManagement.Commands.UpdateSocialMediaLinks
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    [CacheReset(nameof(GetCompanyQuery), "{{CompanyId}}")]
    public class UpdateSocialMediaLinksCommand : IRequest
    {
        [JsonIgnore]
        public Guid CompanyId { get; set; }
        public string Telegram { get; set; }
        public string Youtube { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Medium { get; set; }
    }
}