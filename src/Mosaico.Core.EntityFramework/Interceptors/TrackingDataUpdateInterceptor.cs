using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mosaico.Authorization.Base;
using Mosaico.Core.EntityFramework.Abstractions;
using Mosaico.Domain.Base;

namespace Mosaico.Core.EntityFramework.Interceptors
{
    public class TrackingDataUpdateInterceptor : SaveChangesInterceptor, ISaveChangesCommandInterceptor
    {
        private readonly ICurrentUserContext _currentUserContext;

        public TrackingDataUpdateInterceptor(ICurrentUserContext currentUserContext = null)
        {
            _currentUserContext = currentUserContext;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            foreach (var entry in eventData.Context.ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.MarkCreated(_currentUserContext?.UserId, entry.Entity.CreatedAt);
                        break;
                    case EntityState.Modified:
                        entry.Entity.MarkModified(_currentUserContext?.UserId, entry.Entity.ModifiedAt);
                        break;
                }
            }
            
            return ValueTask.FromResult(result);
        }
    }
}