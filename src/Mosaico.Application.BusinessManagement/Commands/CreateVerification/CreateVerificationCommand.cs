using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Commands.CreateVerification
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class CreateVerificationCommand : IRequest<Guid>
    {
        public string CompanyRegistrationUrl { get; set; }
        public string CompanyAddressUrl { get; set; }
        public List<ShareholderDTO> Shareholders { get; set; } = new();
        public Guid CompanyId { get; set; }
    }
}