using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.UpdateCompanyDocument
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class UpdateCompanyDocumentCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CompanyId { get; set; }
    }
}
