using MongoDB.Bson.Serialization.Attributes;

namespace Nemesis.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Alias { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Birth { get; set; }
        public string? Password { get; set; }
    }
}
