using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProducerService.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TransactionNumber { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Action { get; set; }
        public DateTime UpdateLog { get; set; }
    }
}