using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanies
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetCompaniesQuery : IRequest<GetCompaniesQueryResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string UserId { get; set; }
    }
}