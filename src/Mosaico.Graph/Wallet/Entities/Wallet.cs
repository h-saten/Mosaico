using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Mosaico.Domain.Mongodb.Base.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Graph.Wallet.Entities
{
    public class Wallet : EntityBase
    {
        public string Address { get; set; }
        
        public string Network { get; set; }
        
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string CompanyId { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)] 
        public WalletType Type { get; set; }
    }
}