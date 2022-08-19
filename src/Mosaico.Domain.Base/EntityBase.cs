using System;

namespace Mosaico.Domain.Base
{
    public abstract class EntityBase : IEntity<Guid>
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public long Version { get; set; }
        public Guid Id { get; set; }

        public void MarkModified(string userId, DateTimeOffset? modifiedAt = null)
        {
            if (modifiedAt == null || modifiedAt.Value == DateTimeOffset.MinValue)
            {
                modifiedAt = DateTimeOffset.UtcNow;
            }
            ModifiedAt = modifiedAt.Value;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                ModifiedById = userId;
            }
            Version++;
        }

        public void MarkCreated(string userId, DateTimeOffset? createdAt = null)
        {
            if (createdAt == null || createdAt.Value == DateTimeOffset.MinValue)
            {
                createdAt = DateTimeOffset.UtcNow;
            }

            CreatedAt = createdAt.Value;
            ModifiedAt = createdAt.Value;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                CreatedById = userId;
                ModifiedById = userId;
            }
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
            Version = 1;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            return obj is EntityBase anotherEntity && Id.Equals(anotherEntity.Id);
        }
    }
}