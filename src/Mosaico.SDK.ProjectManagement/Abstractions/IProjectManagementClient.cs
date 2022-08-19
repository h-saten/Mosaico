using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.SDK.ProjectManagement.Models;

namespace Mosaico.SDK.ProjectManagement.Abstractions
{
    public interface IProjectManagementClient
    {
        Task<MosaicoProject> GetProjectAsync(Guid id, CancellationToken token = new CancellationToken());
        Task<MosaicoProject> GetProjectAsync(string title, CancellationToken token = new CancellationToken());
        Task<ProjectStage> CurrentProjectSaleStage(Guid id, CancellationToken token = new CancellationToken());
        Task<MosaicoProjectDetails> GetProjectDetailsAsync(Guid id, CancellationToken token = new CancellationToken());
        Task<MosaicoProjectDetails> GetProjectDetailsAsync(string slug, CancellationToken token = new CancellationToken());
        Task<List<MosaicoProject>> GetActiveProjectsAsync(CancellationToken token = new());
        Task<MosaicoProjectTransaction> GetProjectTransactionAsync(Guid projectId,
            CancellationToken token = new CancellationToken());
        Task<MosaicoProjectDocument> GetProjectDocumentAsync(Guid id, CancellationToken token = new CancellationToken());
        Task<List<MosaicoProject>> GetProjectsByTokenAsync(Guid tokenId, CancellationToken token = new CancellationToken());
        Task<List<MosaicoProject>> GetProjectsOfCompanyAsync(Guid id, int limit = 3, CancellationToken token = new CancellationToken());
        Task<ProjectStage> GetStageAsync(Guid id, CancellationToken token = new CancellationToken());
        Task<List<KeyValuePair<Guid, string>>> GetTokensCrowdSaleAsync(List<Guid> tokensId, CancellationToken token = new());
        Task SetAirdropWithdrawnAsync(Guid id, List<string> userIds, string transactionHash, CancellationToken token = new());
        Task<MosaicoAirdrop> GetProjectAirdropAsync(Guid id, CancellationToken token = new());
        Task<int> SubscribersAmountAsync(Guid id, CancellationToken token = new());
        Task<List<MosaicoProjectInvestor>> GetProjectPrivateInvestorsAsync(Guid id);

        Task<MosaicoProjectDocument> GetProjectDocumentAsync(Guid projectId, string type,
            string lang = Mosaico.Base.Constants.Languages.English, CancellationToken token = new());

        Task<List<string>> GetProjectPaymentCurrenciesAsync(Guid projectId);
        Task<ActiveSaleProject> GetProjectWithActiveSale(Guid tokenId, CancellationToken token = new());
        Task<List<MosaicoAirdrop>> GetStageAirdropsAsync(Guid id, CancellationToken token = new());

    }
}