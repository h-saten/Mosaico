using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.GetInvestmentPackages;
using Mosaico.Application.ProjectManagement.Queries.GetProjectDocument;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertInvestmentPackage
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetInvestmentPackagesQuery), "{{PageId}}_{{Language}}")]
    public class UpsertInvestmentPackageCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid PageId { get; set; }
        
        [JsonIgnore]
        public Guid InvestmentPackageId { get; set; }
        public decimal TokenAmount { get; set; }
        public string Name { get; set; }
        public List<string> Benefits { get; set; }
        public string Language { get; set; }
    }
}