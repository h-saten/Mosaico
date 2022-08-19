using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.DeleteCompanyDocument
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class DeleteCompanyDocumentCommand : IRequest
    {
        public Guid CompanyId { get; set; }
        public Guid Id { get; set; }
    }
}
