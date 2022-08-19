using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyDocument
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    // [Cache("{{CompanyId}}_{{Language}}")]
    public class GetCompanyDocumentQuery: IRequest<DocumentContentDTO>
    {
        [JsonIgnore]
        public Guid CompanyId { get; set; }
        public string Language { get; set; }
    }
}
