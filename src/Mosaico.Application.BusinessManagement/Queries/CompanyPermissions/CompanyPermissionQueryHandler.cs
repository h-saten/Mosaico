using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.BusinessManagement.Permissions;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Extensions;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.CompanyPermissions
{
    public class CompanyPermissionQueryHandler : IRequestHandler<CompanyPermissionQuery, Permissions.CompanyPermissions>
    {
        private readonly ICompanyPermissionFactory _permissionFactory;
        private readonly IBusinessDbContext _businessDbContext;

        public CompanyPermissionQueryHandler(IBusinessDbContext businessDbContext,
            ICompanyPermissionFactory permissionFactory)
        {
            _businessDbContext = businessDbContext;
            _permissionFactory = permissionFactory;
        }

        public async Task<Permissions.CompanyPermissions> Handle(CompanyPermissionQuery request, CancellationToken cancellationToken)
        {
            var project = await _businessDbContext.GetCompanyOrThrowAsync(request.UniqueIdentifier, cancellationToken);
            var permissions = await _permissionFactory.CreateCompanyPermissionsAsync(project, request.UserId, cancellationToken);

            return permissions;
        }
    }
}