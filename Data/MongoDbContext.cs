using library_management.Models;
using MongoDB.Driver;

namespace library_management.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly string _connectionString;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING"); ;
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
        }


        public IMongoCollection<Book> Books => _database.GetCollection<Book>("Books");
        public IMongoCollection<Member> Members => _database.GetCollection<Member>("Members");
    }
}
