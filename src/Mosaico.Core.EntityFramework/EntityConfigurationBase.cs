using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mosaico.Domain.Base;

namespace Mosaico.Core.EntityFramework
{
    public abstract class EntityConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
    { 
        protected abstract string TableName { get; }
        protected abstract string Schema { get; }
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(TableName,Schema);
            builder.HasKey(e => e.Id);
        }
    }
}