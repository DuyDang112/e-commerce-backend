using Contract.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contract.Domain
{
    public class MongoEntity 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public virtual string Id { get; protected init; }
        [BsonElement("createdDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [BsonElement("lastModifieDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? LassModifiedDate {get; set;} = DateTime.UtcNow;
    }
}
