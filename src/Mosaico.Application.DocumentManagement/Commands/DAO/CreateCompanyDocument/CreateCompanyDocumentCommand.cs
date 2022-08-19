using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.CreateCompanyDocument
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class CreateCompanyDocumentCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsMandatory { get; set; } = false;
    }
}
