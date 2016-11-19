using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SQL.NoSQL.Library.Interfaces
{
    public abstract class IEntityBase
    {
        [BsonId]
        public virtual Guid Id { get; set; }
    }
}
