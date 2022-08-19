using System;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.GetInvestmentPackages;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteInvestmentPackage
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetInvestmentPackagesQuery), "{{PageId}}_*")]
    public class DeleteInvestmentPackageCommand : IRequest
    {
        public Guid PageId { get; set; }
        public Guid InvestmentPackageId { get; set; }
    }
}