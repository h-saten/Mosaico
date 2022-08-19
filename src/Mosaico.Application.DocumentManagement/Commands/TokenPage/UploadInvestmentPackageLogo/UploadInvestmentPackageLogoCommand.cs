using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadInvestmentPackageLogo
{
    [Restricted(nameof(PageId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UploadInvestmentPackageLogoCommand : IRequest<string>
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public Guid PageId { get; set; }
        public Guid InvestmentPackageId { get; set; }
    }
}