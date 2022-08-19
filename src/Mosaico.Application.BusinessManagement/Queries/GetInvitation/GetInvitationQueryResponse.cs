using System.Collections.Generic;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Application.BusinessManagement.Queries.GetInvitation
{
    public class GetInvitationQueryResponse
    {
        public CompanyInvitationDTO Invitation { get; set; }
    }
}