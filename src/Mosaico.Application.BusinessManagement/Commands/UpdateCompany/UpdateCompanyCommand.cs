using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Application.BusinessManagement.Queries.GetCompany;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Application.BusinessManagement.Commands.UpdateCompany
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    [CacheReset(nameof(GetCompanyQuery), "{{CompanyId}}")]
    public class UpdateCompanyCommand : IRequest
    {
        public Guid CompanyId { get; set; }
        public string CompanyDescription { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string VATId { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Size { get; set; }
        public string Region { get; set; }
    }
}