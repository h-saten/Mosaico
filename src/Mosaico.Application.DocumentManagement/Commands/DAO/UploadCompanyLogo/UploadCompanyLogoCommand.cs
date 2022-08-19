using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.UploadCompanyLogo
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class UploadCompanyLogoCommand : IRequest<string>
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public Guid CompanyId { get; set; }
    }
}