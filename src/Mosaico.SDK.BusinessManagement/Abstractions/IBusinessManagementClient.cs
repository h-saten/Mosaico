using Mosaico.SDK.BusinessManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.SDK.BusinessManagement.Abstractions
{
    public interface IBusinessManagementClient
    {
        Task<MosaicoCompany> GetCompanyAsync(Guid id, CancellationToken token = new CancellationToken());
        Task DeleteUserMembershipAsync(string userId, CancellationToken token = new CancellationToken());
        Task SetContractAddressAsync(Guid id, string contractAddress, CancellationToken token = new CancellationToken());
        Task DeleteCompanyAsync(Guid id, CancellationToken token = new CancellationToken());
    }
}
