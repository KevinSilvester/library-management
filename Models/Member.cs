using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace library_management.Models
{
    public class Member
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Map ObjectId to string
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("membershipDate")]
        public DateTime MembershipDate { get; set; }
    }
}
