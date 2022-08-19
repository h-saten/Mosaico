using System;
using System.Threading.Tasks;
using Mosaico.Domain.BusinessManagement.Entities;

namespace Mosaico.Application.BusinessManagement.Services
{
    public interface IProposalService
    {
        Task<Guid> VoteAsync(Proposal proposal, VoteResult result, string userId);
    }
}