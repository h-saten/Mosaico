using System;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.SDK.DocumentManagement.Models;

namespace Mosaico.SDK.DocumentManagement.Abstractions
{
    public interface IDocumentManagementClient
    {
        Task<CompanyLogo> GetCompanyLogo(Guid companyId, CancellationToken token = new CancellationToken());
    }
}