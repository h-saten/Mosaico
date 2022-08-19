using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mosaico.Domain.Mongodb.Base.Models
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }
        
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}