using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Application.ProjectManagement.Services.Models;

namespace Mosaico.Application.ProjectManagement.Abstractions
{
    public interface IAirdropImportService
    {
        Task<List<AirdropImportRecord>> GetAirdropParticipantsAsync(byte[] file);
    }
}