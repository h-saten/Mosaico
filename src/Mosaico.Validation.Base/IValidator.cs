using System;
using System.Threading.Tasks;
using Mosaico.Domain.Base;

namespace Mosaico.Validation.Base
{
    public interface IValidator<TEntity> where TEntity : class, IEntity<Guid>
    {
        Task ValidateOnCreateAsync(TEntity entity);
        Task ValidateOnUpdateAsync(TEntity entity);
        Task ValidateOnDeleteAsync(TEntity entity);
    }
    
    public interface IValidator<TEntity, TId> where TEntity : class, IEntity<TId>
    {
        Task ValidateOnCreateAsync(TEntity entity);
        Task ValidateOnUpdateAsync(TEntity entity);
        Task ValidateOnDeleteAsync(TEntity entity);
    }
}