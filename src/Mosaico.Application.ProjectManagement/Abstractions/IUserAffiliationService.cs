using System.Threading.Tasks;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;

namespace Mosaico.Application.ProjectManagement.Abstractions
{
    public interface IUserAffiliationService
    {
        Task<UserAffiliation> GetOrCreateUserAffiliation(string userId);
    }
}