using System;
using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.GetInvestmentPackages
{
    // [Cache("{{Id}}_{{Language}}")]
    public class GetInvestmentPackagesQuery : IRequest<GetInvestmentPackagesQueryResponse>
    {
        public Guid Id { get; set; }
        public string Language { get; set; }
    }
}