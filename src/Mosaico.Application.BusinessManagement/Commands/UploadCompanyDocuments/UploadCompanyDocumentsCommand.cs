using Mosaico.Authorization.Base;
using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Mosaico.Application.BusinessManagement.Commands.UploadCompanyDocuments
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class UploadCompanyDocumentsCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid CompanyId { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public byte[] Content { get; set; }
    }
}
