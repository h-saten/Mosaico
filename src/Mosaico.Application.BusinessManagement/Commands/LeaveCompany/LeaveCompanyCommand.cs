using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Commands.LeaveCompany
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanRead)]
    public class LeaveCompanyCommand : IRequest
    {
        public Guid CompanyId { get; set; }
    }
}