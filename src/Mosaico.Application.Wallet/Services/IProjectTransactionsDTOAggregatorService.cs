using System.Threading.Tasks;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services
{
    public interface IProjectTransactionsDTOAggregatorService
    {
        Task<ProjectTransactionDTO> FillInDTOAsync(Transaction transaction);
    }
}