using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace iwip.MongoDB;

public class BaseEntity : AuditedAggregateRoot<Guid>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonProperty("_id")]
    public override Guid Id { get; protected set; }
}
