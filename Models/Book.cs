using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace library_management.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Maps ObjectId to a string
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("isbn")]
        public string ISBN { get; set; }

        [BsonElement("genre")]
        public string Genre { get; set; }

        [BsonElement("copiesAvailable")]
        public int CopiesAvailable { get; set; }
    }
}
