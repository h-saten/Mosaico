using System;
using MediatR;

namespace Mosaico.Application.BusinessManagement.Queries.CompanyPermissions
{
    public class CompanyPermissionQuery : IRequest<Permissions.CompanyPermissions>
    {
        public string UserId { get; set; }
        public string UniqueIdentifier { get; set; }
    }
}